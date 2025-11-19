## РОЗДІЛ 3. ПРОГРАМНА ДОКУМЕНТАЦІЯ

### 3.1 Документування вимог до програмної системи

#### 3.1.1 Формат User Stories

Перелік можливостей, які доступні користувачам з різними ролями в системі, наведено в табл.5.

**Таблиця 5 – Можливості користувача (User Requirements)**

| User Requirements ID | Description of user capabilities (User Requirements) |
|----------------------|------------------------------------------------------|
| UR.1 | Система повинна надавати можливість реєстрації нового облікового запису користувача (введення паспортних даних, логіна, email та пароля). |
| UR.2 | Система повинна надавати можливість авторизації користувача за логіном і паролем з урахуванням його ролі (гість, авторизований, оператор, адміністратор). |
| UR.3 | Система повинна забезпечувати функцію відновлення пароля на основі одноразового коду, надісланого на email користувача. |
| UR.4 | Система повинна дозволяти користувачу переглядати та змінювати дані свого облікового запису (роль, ПІБ, паспортні дані, пароль). |
| UR.5 | Система повинна відображати довідкову інформацію та інструкції, адаптовані до поточних прав доступу користувача. |
| UR.6 | Система повинна дозволяти гостю переглядати каталог усіх автомобілів з основними характеристиками (бренд, модель, рік, ціна, пробіг, кузов, привід, КПП, колір, тип двигуна). |
| UR.7 | Система повинна дозволяти гостю переглядати сторінку власного профілю з відображенням поточних прав доступу. |
| UR.8 | Система повинна дозволяти гостю формувати та надсилати запит на підвищення прав доступу до рівня «Авторизований». |
| UR.9 | Система повинна надавати авторизованому користувачу можливість переглядати та фільтрувати каталог товарів (продуктів/автомобілів), доступних до продажу. |
| UR.10 | Система повинна дозволяти авторизованому користувачу створювати замовлення на купівлю вибраного автомобіля із зазначенням типу оплати (готівка/картка/кредит), необхідності доставки, адреси та номера телефону. |
| UR.11 | Система повинна відображати авторизованому користувачу перелік усіх його замовлень із зазначенням автомобіля, дати створення та деталей замовлення. |
| UR.12 | Система повинна дозволяти авторизованому користувачу скасовувати власне замовлення до його остаточного погодження оператором. |
| UR.13 | Система повинна дозволяти авторизованому користувачу оновлювати параметри вже створеного замовлення (тип оплати, доставка, адреса тощо), якщо це дозволяє поточний статус замовлення. |
| UR.14 | Система повинна відображати авторизованому користувачу статус розгляду його замовлень оператором та текст причини можливого відхилення. |
| UR.15 | Система повинна підтримувати оплату схвалених замовлень банківською карткою з автоматичним формуванням електронної квитанції. |
| UR.16 | Система повинна надавати можливість переглядати деталі сформованої платіжної квитанції за кожним замовленням. |
| UR.17 | Система повинна надавати авторизованому користувачу аналітичний звіт щодо автомобілів з мінімальним залишком на складі, щоб виявляти «дефіцитні» моделі. |
| UR.18 | Система повинна надавати авторизованому користувачу аналітику щодо затримок поставок та брендів, щоб оцінювати надійність постачальників. |
| UR.19 | Система повинна дозволяти оператору переглядати список автомобілів, які ще не прив’язані до товарних позицій, і створювати для них продукти (виводити на продаж). |
| UR.20 | Система повинна дозволяти оператору додавати нові автомобілі до бази даних із заповненням усіх технічних характеристик (тип двигуна, потужність, колір, кузов, коробка передач, вага, пробіг, рік випуску тощо). |
| UR.21 | Система повинна дозволяти оператору переглядати, редагувати та видаляти товарні позиції (продукти), а також фільтрувати їх за статусом «в наявності/немає». |
| UR.22 | Система повинна дозволяти оператору переглядати список замовлень, які ще не були розглянуті, та приймати рішення щодо кожного (схвалити/відхилити) із фіксацією причини відхилення. |
| UR.23 | Система повинна надавати оператору доступ до детальної інформації про клієнтів (ПІБ, паспортні дані, роль, пов’язаний користувач), щоб забезпечити коректну ідентифікацію покупців. |
| UR.24 | Система повинна надавати оператору звіт «Користувачі та автомобілі за типом оплати» для аналізу попиту за способами оплати. |
| UR.25 | Система повинна надавати оператору аналітичні звіти щодо популярності моделей за квартал, кількості клієнтів, що очікують автомобілі, кількості контрактів із дилерами та статистики цін за брендами. |
| UR.26 | Система повинна дозволяти адміністратору переглядати запити користувачів на підвищення прав доступу та змінювати роль користувача на «Авторизований» у разі схвалення такого запиту. |
| UR.27 | Система повинна дозволяти адміністратору створювати нові облікові записи операторів та підвищувати роль існуючих авторизованих користувачів до рівня «Оператор». |
| UR.28 | Система повинна надавати адміністратору доступ до всіх функцій оператора (керування товарами, замовленнями, клієнтами, платежами та аналітичними звітами). |
| UR.29 | Система повинна надавати адміністратору SQL-консоль для виконання довільних SELECT-запитів до бази даних, перегляду результатів у табличному вигляді та редагування окремих записів із забороною небезпечних DDL-операцій (CREATE/ALTER/DROP тощо). |

Перелік функціональності системи з точки зору кінцевого користувача, у вигляді користувацьких історій (User Story), наведено в табл.6.

**Таблиця 6 – Перелік User Story**

| User Story ID | AS A `<type of user>` | I WANT TO `<perform some task>` | SO THAT I CAN `<achieve some goal>` |
|---------------|-----------------------|----------------------------------|-------------------------------------|
| US.1 | AS A user | I WANT TO register a new account with my personal and passport data | SO THAT I CAN get access to the system and its services |
| US.2 | AS A user | I WANT TO log in using my login and password | SO THAT I CAN work with the functionality that matches my role (guest/authorized/operator/admin) |
| US.3 | AS A user | I WANT TO reset my password via a verification code sent to my email | SO THAT I CAN restore access to my account if I forget the password |
| US.4 | AS A user | I WANT TO view and edit my profile data (role, name, passport, password) | SO THAT I CAN keep my account information up to date and secure |
| US.5 | AS A user | I WANT TO open role-specific help and instructions | SO THAT I CAN quickly understand how to use the interface and available features |
| US.6 | AS A guest | I WANT TO browse a list of all cars with main characteristics | SO THAT I CAN get acquainted with the dealership offers |
| US.7 | AS A guest | I WANT TO view my profile with current access rights | SO THAT I CAN understand my current status in the system |
| US.8 | AS A guest | I WANT TO send a request to extend my rights to “Authorized” | SO THAT I CAN place orders for cars |
| US.9 | AS AN authorized user | I WANT TO browse and filter the catalog of cars/products | SO THAT I CAN quickly find a car that matches my preferences |
| US.10 | AS AN authorized user | I WANT TO initiate the purchase of a selected car, specifying payment type and delivery details | SO THAT I CAN place an order for the desired car |
| US.11 | AS AN authorized user | I WANT TO view the list of all my orders with details | SO THAT I CAN track my purchases and their parameters |
| US.12 | AS AN authorized user | I WANT TO cancel my order before it is finally approved by an operator | SO THAT I CAN change my decision without financial consequences |
| US.13 | AS AN authorized user | I WANT TO update payment and delivery parameters of an existing order | SO THAT I CAN correct mistakes or change conditions while the order is still editable |
| US.14 | AS AN authorized user | I WANT TO see the review status of my orders and reasons for rejection | SO THAT I CAN understand what happened with my request and what to fix |
| US.15 | AS AN authorized user | I WANT TO pay for an approved order by card and get an electronic receipt | SO THAT I CAN complete the purchase and have official payment confirmation |
| US.16 | AS AN authorized user | I WANT TO view details of generated receipts in the system | SO THAT I CAN verify the payment information when needed |
| US.17 | AS AN authorized user | I WANT TO see analytical reports about cars with minimal stock and delayed supplies | SO THAT I CAN understand which models are scarce or problematic to supply |
| US.18 | AS AN operator | I WANT TO see cars that are not yet bound to products and put them on sale | SO THAT I CAN prepare the full assortment for customers |
| US.19 | AS AN operator | I WANT TO add new cars with full technical characteristics | SO THAT I CAN keep the database of vehicles complete and up to date |
| US.20 | AS AN operator | I WANT TO edit and delete product items and filter them by stock status | SO THAT I CAN manage prices, availability and correctness of product data |
| US.21 | AS AN operator | I WANT TO review all pending customer orders and approve or reject them with a reason | SO THAT I CAN control which orders are fulfilled and provide feedback to clients |
| US.22 | AS AN operator | I WANT TO see detailed information about clients and their documents | SO THAT I CAN correctly identify customers during order processing and payments |
| US.23 | AS AN operator | I WANT TO view payment history and open or save PDF receipts | SO THAT I CAN handle customer requests regarding their payments and documents |
| US.24 | AS AN operator | I WANT TO view reports on users, cars and payment types, as well as other analytical dashboards | SO THAT I CAN analyse demand, popular models and dealer contracts |
| US.25 | AS AN administrator | I WANT TO review access-upgrade requests from users and approve or reject them | SO THAT I CAN control who receives extended rights in the system |
| US.26 | AS AN administrator | I WANT TO create operator accounts and promote existing users to operator role | SO THAT I CAN ensure the dealership has enough staff with appropriate rights |
| US.27 | AS AN administrator | I WANT TO have access to all operator functions (products, orders, clients, payments, analytics) | SO THAT I CAN supervise and, if necessary, perform operational tasks myself |
| US.28 | AS AN administrator | I WANT TO execute SELECT queries in the SQL console and edit result rows where allowed | SO THAT I CAN perform advanced analysis and corrections without dangerous DDL operations |

