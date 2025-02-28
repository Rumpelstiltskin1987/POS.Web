CREATE TABLE Category (
    IdCategory INTEGER PRIMARY KEY AUTOINCREMENT,  				-- Identificador único de la categoría
    Name TEXT NOT NULL,                           				-- Nombre de la categoría
	Status TEXT NOT NULL CHECK(Status IN ('AC', 'IN')),			-- AC (Activo), IN (Inactivo)
	CreateUser TEXT  NOT NULL,									-- Usuario de creación del registro
	CreateDate TEXT NOT NULL,									-- Fecha de creación del registro
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT 										-- Feha de modificación del registro
);

CREATE TABLE CategoryLog (
	IdMovement INTEGER NOT NULL,								-- Numero de log para una categoría (consecutivo)
	IdCategory INTEGER NOT NULL,  								-- Identificador único de la categoría
    Name TEXT NOT NULL,                           				-- Nombre de la categoría
	Status TEXT NOT NULL CHECK(Status IN ('AC', 'IN')),			-- AC (Activo), IN (Inactivo)
	MovementType TEXT NOT NULL CHECK(MovementType IN('AL','ED','EL')),-- Tipo de movimiento Alta(AL), Edicion(ED), Eliminacion (EL)
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT,										-- Feha de modificación del registro
	PRIMARY KEY (IdMovement, IdCategory), 						-- Clave primaria compuesta
	FOREIGN KEY (IdCategory) REFERENCES Category(IdCategory) 	-- Relación con Categoría
);

CREATE TABLE Product (
    IdProduct INTEGER PRIMARY KEY AUTOINCREMENT, 				-- Identificador único del producto
    Name TEXT NOT NULL,                        					-- Nombre del producto
	Description TEXT,											-- Descripcion del producto
    IdCategory INTEGER NOT NULL,               					-- Categoría del producto (FK a Categorías)
    Price REAL NOT NULL CHECK(Price >= 0),                      -- Precio por unidad del producto    	
    MeasureUnit TEXT NOT NULL,                 					-- Unidad de medida (ej.: kg, pieza, litro)
	UrlImage TEXT NOT NULL,										-- Ruta donde se guarda la imagen de producto
	Status TEXT NOT NULL CHECK(Status IN ('AC', 'IN')),			-- AC (Activo), IN (Inactivo)
	CreateUser TEXT  NOT NULL,									-- Usuario de creación del registro
	CreateDate TEXT NOT NULL,									-- Fecha de creación del registro
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT,										-- Feha de modificación del registro
    FOREIGN KEY (IdCategory) REFERENCES Category(IdCategory) 	-- Relación con Categoría
);

CREATE TABLE ProductLog (
	IdMovement INTEGER NOT NULL,								-- Numero de log para una categoría (consecutivo)
    IdProduct INTEGER NOT NULL, 								-- Identificador único del producto
    Name TEXT NOT NULL,                        					-- Nombre del producto
	Description TEXT,											-- Descripcion del producto
    IdCategory INTEGER NOT NULL,               					-- Categoría del producto (FK a Categorías)
    Price REAL NOT NULL CHECK(Price >= 0),                      -- Precio por unidad del producto
    MeasureUnit TEXT NOT NULL,                 					-- Unidad de medida (ej.: kg, pieza, litro)
	UrlImage TEXT NOT NULL,										-- Ruta donde se guarda la imagen de producto
	Status TEXT NOT NULL CHECK(Status IN ('AC', 'IN')),			-- AC (Activo), IN (Inactivo)
	MovementType TEXT NOT NULL CHECK(MovementType IN('AL','ED','EL')),-- Tipo de movimiento Alta(AL), Edicion(ED), Eliminacion (EL)
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT,										-- Feha de modificación del registro
	PRIMARY KEY (IdMovement, IdProduct, Name, IdCategory), 		-- Clave primaria compuesta
    FOREIGN KEY (IdCategory) REFERENCES Category(IdCategory), 	-- Relación con Categoría
	FOREIGN KEY (IdProduct) REFERENCES Product(IdProduct)      	-- Relación con Producto
);

CREATE TABLE Warehouse (
	IdWarehouse INTEGER PRIMARY KEY AUTOINCREMENT,				-- Identificador único de almacen
    Name TEXT NOT NULL,											-- Nombre del almacen
 	IdWL INTEGER NOT NULL,										-- Identifidor de la ubicacion del almacen
	CreateUser TEXT  NOT NULL,									-- Usuario de creación del registro
	CreateDate TEXT NOT NULL,									-- Fecha de creación del registro
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT,										-- Feha de modificación del registro
	FOREIGN KEY (IdWL) REFERENCES WarehouseLocation(IdWL)       -- Relación con la dirección del almacén	
);

CREATE TABLE WarehouseLocation (
    IdWL INTEGER PRIMARY KEY AUTOINCREMENT,				        -- Identificador único de la ubicación de almacen
    Address TEXT NOT NULL										-- Dirección del almacen.
);

CREATE TABLE Stock (
	IdStock INTEGER PRIMARY KEY AUTOINCREMENT,	
	IdWarehouse INTEGER NOT NULL,
	IdProduct INTEGER NOT NULL,
	IdCategory INTEGER NOT NULL,
	Quantity INTEGER NOT NULL,
	CreateUser TEXT  NOT NULL,									
	CreateDate TEXT NOT NULL,									
	LastUpdateUser TEXT,
	LastUpdateDate TEXT,
	FOREIGN KEY (IdProduct) REFERENCES Product(IdProduct),
	FOREIGN KEY (IdCategory) REFERENCES Category(IdCategory),
	FOREIGN KEY (IdWarehouse) REFERENCES Warehouse(IdWarehouse),
	UNIQUE (IdProduct,IdWarehouse)	
);

CREATE TABLE Inventory (
    IdMovement INTEGER NOT NULL, 				-- Identificador único del movimiento
    IdStock INTEGER NOT NULL,                    				-- Referencia al producto afectado
    MovementType TEXT NOT NULL CHECK(MovementType IN ('Entrada', 'Salida', 'Ajuste')), -- Tipo de movimiento (Entrada/Salida)
    Quantity REAL NOT NULL,                         			-- Cantidad de producto movida
	Description TEXT NOT NULL,
	MovementUser TEXT NOT NULL,									-- Usuario que realiza el movimiento
    MovementDate TEXT NOT NULL,                  				-- Fecha del movimiento (ISO 8601)
	PRIMARY KEY (IdMovement, IdStock),
    FOREIGN KEY (IdStock) REFERENCES Stock(IdStock) 			-- Relación con Productos
);

CREATE TABLE Sales (
    IdSales INTEGER PRIMARY KEY AUTOINCREMENT, 					-- Identificador único de la venta
    SalesDate TEXT NOT NULL,                  					-- Fecha de la venta (formato ISO 8601: YYYY-MM-DD HH:MM:SS)
    Total REAL NOT NULL CHECK(Total >= 0),     					-- Total de la venta
	CreateUser TEXT  NOT NULL,									-- Usuario de creación del registro
	CreateDate TEXT NOT NULL,									-- Fecha de creación del registro
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT											-- Feha de modificación del registro
);

CREATE TABLE SalesDetail (
    IdSalesDetail INTEGER PRIMARY KEY AUTOINCREMENT, 			-- Identificador único del detalle de venta
    IdSales INTEGER NOT NULL,                         			-- Referencia a la tabla Ventas
    IdProduct INTEGER NOT NULL,                      			-- Referencia a la tabla Productos
    Quantity REAL NOT NULL,                           			-- Cantidad de producto vendido
    Subtotal REAL NOT NULL ,                           			-- Subtotal (Cantidad * Precio)
	CreateUser TEXT NOT NULL,									-- Usuario de creación del registro
	CreateDate TEXT NOT NULL,									-- Fecha de creación del registro
	LastUpdateUser TEXT,										-- Usuario de modificación del registro
	LastUpdateDate TEXT,											-- Feha de modificación del registro
    FOREIGN KEY (IdSales) REFERENCES Sales(IdSales), 			-- Relación con Ventas
    FOREIGN KEY (IdProduct) REFERENCES Product(IdProduct) 		-- Relación con Productos
);


