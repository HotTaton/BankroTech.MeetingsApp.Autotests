﻿@Интерфейс
Feature: Создание собрания
	Проверяем создание собрания

Background: 
	Given я авторизованный пользователь

@Интерфейс
Scenario: Переход к созданию собрания	
	Given я захожу на страницу "Список собраний"	
	When я нажимаю на кнопку "Создать собрание"
	Then открывается новая вкладка со страницей "Создание собрания"

@Интерфейс
Scenario: Создание собрания
	Given я захожу на страницу "Создание собрания"	
	And ввожу в поле "Наименование" данные: "Собрание по банкротству должника Тимофеева К.В."
	And ввожу в поле "Дата начала" данные: "<Дата, завтра>"
	And ввожу в поле "Время начала" данные: "15:00"
	And ввожу в поле "Дата окончания" данные: "<Дата, послезавтра>"
	And ввожу в поле "Время окончания" данные: "16:00"
	And я раскрываю панель "Должник"
	And ввожу в поле "Номер дела" данные: "<Случайное число, 8 цифр>"
	And ввожу в поле "Арбитражный суд" данные: "Арбитражный суд Алтайского края"
	And ввожу в поле "Краткое наименование должника" данные: "Кирюндель"
	And ввожу в поле "ИНН" данные: "123455555555"
	And ввожу в поле "ОГРН" данные: "123456666677788"
	And ввожу в поле "Полное наименование должника" данные: "Кирилл 'Должничок' Тимофеев"
	And ввожу в поле "Адрес" данные: "Нариманова 2г, кв.82"
	When я нажимаю на кнопку "Сохранить"	
	And сохраняю параметр "Id" из результата запроса "/api/meeting/create[0].Result.Id"
	Then я перехожу на страницу "Текущее собрание"
	And выполняю запрос 
	"""
	SELECT * FROM "Meetings"
	WHERE "Id" = '<Параметр, /api/meeting/create[0].Result.Id>'
	"""
	And вижу следующие данные
	| Name                                            | Debtor_FullName             | Debtor_ShortName |
	| Собрание по банкротству должника Тимофеева К.В. | Кирилл 'Должничок' Тимофеев | Кирюндель        |