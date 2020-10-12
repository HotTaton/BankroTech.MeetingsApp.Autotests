Feature: Проверка апи
	Проверяем как работает API

Scenario: Логин
	Given посылаю запрос "/account/login" с телом
	"""
	{
	  "PhoneNumber": "79171864323",
	  "Password": "12345678"
	}
	"""
	And выполняю запрос
	"""
	SELECT "Value"
	FROM "AccountTokens"
	ORDER BY "ExpiresAt" DESC
	LIMIT 1
	"""
	When посылаю запрос "/account/verifyCode" с телом
	"""
	{
	  "Code": "<Параметр, Результат SQL запроса[0].Value>"
	}
	"""
	Then результат "/account/verifyCode[0].IsSuccess" истина