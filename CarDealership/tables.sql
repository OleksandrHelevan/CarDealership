-- Таблиця "engines"
create table engines
(
    id               serial
        primary key,
    engine_type      text             not null,
    power            double precision not null,
    fuel_type        text,
    fuel_consumption real,
    battery_capacity double precision,
    range            integer,
    motor_type       text
);

alter table engines
    owner to oleksandr;

-- Таблиця "keys"
create table keys
(
    id           serial
        primary key,
    login        varchar(100) not null
        unique,
    password     varchar(100) not null,
    access_right text         not null,
    email        varchar(255)
);

alter table keys
    owner to oleksandr;

--  Таблиця "cars"
create table cars
(
    id              serial
        primary key,
    car_type        text                  not null,
    brand           varchar(100)          not null,
    model_name      varchar(100)          not null,
    engine_id       integer               not null
        constraint fk_cars_engines
            references engines
            on delete cascade,
    color           text                  not null,
    mileage         integer               not null,
    price           numeric(18, 2)        not null,
    weight          integer               not null,
    drive_type      text                  not null,
    transmission    text                  not null,
    year            integer               not null,
    number_of_doors integer               not null,
    body_type       text                  not null,
    on_sale         boolean default false not null
);

alter table cars
    owner to oleksandr;

create index idx_cars_engine_id
    on cars (engine_id);

-- Таблиця "products"
create table products
(
    id                serial
        primary key,
    number            text              not null,
    country_of_origin text              not null,
    in_stock          boolean           not null,
    available_from    timestamp,
    car_id            integer           not null
        constraint fk_products_cars
            references cars
            on delete cascade,
    amount            integer default 1 not null
);

alter table products
    owner to oleksandr;

create index idx_products_car_id
    on products (car_id);

-- Таблиця "passport_data"
create table passport_data
(
    id              serial
        primary key,
    first_name      text not null,
    last_name       text not null,
    passport_number text not null
        unique
);

alter table passport_data
    owner to oleksandr;

-- Таблиця "clients"
create table clients
(
    id               serial
        primary key,
    user_id          integer not null
        constraint fk_clients_keys
            references keys
            on delete cascade,
    passport_data_id integer not null
        constraint fk_clients_passport
            references passport_data
            on delete cascade
);

alter table clients
    owner to oleksandr;

create index idx_clients_user_id
    on clients (user_id);

-- Таблиця "orders"
create table orders
(
    id           serial
        primary key,
    client_id    integer   not null
        constraint fk_orders_clients
            references clients
            on delete restrict,
    product_id   integer   not null
        constraint fk_orders_products
            references products
            on delete restrict,
    order_date   timestamp not null,
    payment_type text      not null,
    delivery     boolean   not null,
    address      varchar(400),
    phone_number varchar(32)
);

alter table orders
    owner to oleksandr;

create index idx_orders_client_id
    on orders (client_id);

create index idx_orders_product_id
    on orders (product_id);

-- Таблиця "requests"
create table requests
(
    id      serial
        primary key,
    user_id integer not null
        constraint fk_requests_keys
            references keys
            on delete cascade,
    status  text    not null
);

alter table requests
    owner to oleksandr;

create index idx_requests_user_id
    on requests (user_id);

-- Таблиця "order_reviews"
create table order_reviews
(
    id                        serial
        primary key,
    order_id                  integer                  not null
        constraint fk_order_reviews_orders
            references orders
            on delete cascade,
    status                    text                     not null,
    message                   varchar(500),
    requires_card_number      boolean                  not null,
    card_number               varchar(32),
    created_at                timestamp with time zone not null,
    updated_at                timestamp with time zone,
    requires_delivery_address boolean default false    not null,
    approved_by_user_id       integer
        constraint fk_order_reviews_approved_by
            references keys
            on delete set null
);

alter table order_reviews
    owner to oleksandr;

create index idx_or_approved_by
    on order_reviews (approved_by_user_id);

-- Таблиця "payment_history"
create table payment_history
(
    id           serial
        primary key,
    order_id     integer                                not null
        constraint fk_payment_history_orders
            references orders
            on delete cascade,
    amount       numeric(18, 2)                         not null,
    card_last4   varchar(8)                             not null,
    created_at   timestamp with time zone default now() not null,
    content_type varchar(100)             default 'application/pdf'::character varying not null,
    receipt_pdf  bytea                                  not null,
    operator_id  integer
        constraint fk_payment_history_operator
            references keys
            on delete set null
);

alter table payment_history
    owner to oleksandr;

create index ix_payment_history_order_id
    on payment_history (order_id);

create index idx_ph_operator
    on payment_history (operator_id);