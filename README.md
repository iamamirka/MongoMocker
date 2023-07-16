#### Тесты на BalanceController написаны в классе BalanceControllerTests.

В классе есть места, где в переменную передается значение константы, это сделано осознанно.
В случае с реальной базой данных для того, чтобы убедиться в том, что значение апдейтится,
нужно было бы сделать запрос в базу данных, получить текущее значение и в конце теста убедиться в том,
что оно было изменено соответствующим методом

#### Для мока базы данных MongoDB использовал библиотеку Moq

### Что сделал? :)
#### Повысил читабельность тестов
- Разбил изначальный тест на несколько тестов (для уменьшения количества энтропии)
- Разделил тесты на логические блоки Arrange, Act, Assert
- Использовал атрибуты [Theory] и [InlineData] для параметризации тестов
- Использовал подробные названия тестов
- Использовал библиотеку FluentAssertions

#### Провел рефакторинг
- Переименовал методы контроллера, дал им более информативные и читабельные названия
- Переименовал сущность DB в AccountsService
- Выделил интерфейс IAccountsService
- Реализовал интерфейс IAccountsService в классе AccountsService

#### Цель создания интерфейса IAccountsService
В контексте данного задания это было нужно для мокирования базы данных