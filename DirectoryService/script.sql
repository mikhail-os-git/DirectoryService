CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
CREATE TABLE departments (
    id uuid NOT NULL,
    name character varying(150) NOT NULL,
    identifier character varying(150) NOT NULL,
    parent_id uuid,
    path text NOT NULL,
    depth smallint NOT NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    CONSTRAINT pk_departments PRIMARY KEY (id),
    CONSTRAINT "FK_departments_departments_parent_id" FOREIGN KEY (parent_id) REFERENCES departments (id) ON DELETE RESTRICT
);

CREATE TABLE locations (
    id uuid NOT NULL,
    name character varying(120) NOT NULL,
    timezone text NOT NULL,
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    address jsonb NOT NULL,
    CONSTRAINT pk_loction PRIMARY KEY (id)
);

CREATE TABLE positions (
    id uuid NOT NULL,
    name character varying(100) NOT NULL,
    description character varying(100),
    is_active boolean NOT NULL DEFAULT TRUE,
    created_at timestamp with time zone NOT NULL,
    updated_at timestamp with time zone NOT NULL,
    CONSTRAINT pk_position PRIMARY KEY (id)
);

CREATE TABLE department_location (
    id uuid NOT NULL,
    department_id uuid NOT NULL,
    location_id uuid NOT NULL,
    CONSTRAINT pk_department_location PRIMARY KEY (id),
    CONSTRAINT "FK_department_location_departments_department_id" FOREIGN KEY (department_id) REFERENCES departments (id) ON DELETE CASCADE,
    CONSTRAINT "FK_department_location_locations_location_id" FOREIGN KEY (location_id) REFERENCES locations (id) ON DELETE CASCADE
);

CREATE TABLE department_position (
    id uuid NOT NULL,
    department_id uuid NOT NULL,
    position_id uuid NOT NULL,
    CONSTRAINT pk_department_position PRIMARY KEY (id),
    CONSTRAINT "FK_department_position_departments_department_id" FOREIGN KEY (department_id) REFERENCES departments (id) ON DELETE CASCADE,
    CONSTRAINT "FK_department_position_positions_position_id" FOREIGN KEY (position_id) REFERENCES positions (id) ON DELETE CASCADE
);

CREATE INDEX "IX_department_location_location_id" ON department_location (location_id);

CREATE UNIQUE INDEX ux_department_location ON department_location (department_id, location_id);

CREATE INDEX "IX_department_position_position_id" ON department_position (position_id);

CREATE UNIQUE INDEX ux_department_position ON department_position (department_id, position_id);

CREATE INDEX "IX_departments_parent_id" ON departments (parent_id);

CREATE UNIQUE INDEX ux_locations_name ON locations (name);

CREATE UNIQUE INDEX ux_position_name ON positions (name);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260130140214_Initial', '9.0.1');

COMMIT;

