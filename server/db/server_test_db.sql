CREATE DATABASE IF NOT EXISTS Hostel CHARACTER SET utf8 COLLATE utf8_general_ci;
USE Hostel;

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

--
-- База данных: `hostel`
--

-- --------------------------------------------------------

--
-- Структура таблицы `hostel`
--

CREATE TABLE `hostel` (
  `id_hostel` int(11) NOT NULL,
  `address` text NOT NULL,
  `name` text NOT NULL,
  `site` text,
  `longitude` double NOT NULL,
  `latitude` double NOT NULL,
  `date_add` datetime DEFAULT CURRENT_TIMESTAMP,
  `date_update` timestamp NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `hostel`
--

INSERT INTO `hostel` (`id_hostel`, `address`, `name`, `site`, `longitude`, `latitude`, `date_add`, `date_update`) VALUES
(1, 'Марата ул., д.25', 'Жить хорошо', 'http://vk.com/otelhorosho\n', 59.927391, 30.352618, '2017-01-18 11:31:46', '2017-01-18 08:31:46'),
(2, 'Большая Конюшенная ул., д.13Н', 'Stay Simple\r\n', 'http://vk.com/staysimple\n', 59.939135, 30.322498, '2017-01-18 18:06:33', '2017-01-18 15:06:33');

-- --------------------------------------------------------

--
-- Структура таблицы `hostel2metro`
--

CREATE TABLE `hostel2metro` (
  `id_hostel2metro` int(11) NOT NULL,
  `id_hostel` int(11) NOT NULL,
  `id_metro` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `hostel2metro`
--

INSERT INTO `hostel2metro` (`id_hostel2metro`, `id_hostel`, `id_metro`) VALUES
(1, 1, 1),
(2, 2, 2);

-- --------------------------------------------------------

--
-- Структура таблицы `hostel2phone`
--

CREATE TABLE `hostel2phone` (
  `id_hostel2phone` int(11) NOT NULL,
  `id_hostel` int(11) NOT NULL,
  `id_phone` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `hostel2phone`
--

INSERT INTO `hostel2phone` (`id_hostel2phone`, `id_hostel`, `id_phone`) VALUES
(1, 1, 1),
(2, 1, 2),
(3, 2, 3);

-- --------------------------------------------------------

--
-- Структура таблицы `metro`
--

CREATE TABLE `metro` (
  `id_metro` int(11) NOT NULL,
  `name` text NOT NULL,
  `longitude` double NOT NULL,
  `latitude` double NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `metro`
--

INSERT INTO `metro` (`id_metro`, `name`, `longitude`, `latitude`) VALUES
(1, 'Владимирская', 59.92746375, 30.34817149),
(2, 'Невский проспект', 59.93557875, 30.32702549);

-- --------------------------------------------------------

--
-- Структура таблицы `phone`
--

CREATE TABLE `phone` (
  `id_phone` int(11) NOT NULL,
  `phone` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `phone`
--

INSERT INTO `phone` (`id_phone`, `phone`) VALUES
(1, '+7 (812) 456-44-34\r\n\r\n'),
(2, '+7 (950) 224-40-00\r\n'),
(3, '+7 (812) 989-71-99 ');

-- --------------------------------------------------------

--
-- Структура таблицы `result`
--

CREATE TABLE `result` (
  `id_result` int(11) NOT NULL,
  `description` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `result`
--

INSERT INTO `result` (`id_result`, `description`) VALUES
(0, 'Регистрация пользователя'),
(1, 'Просмотр хостела'),
(2, 'Звонок в хостел');

-- --------------------------------------------------------

--
-- Структура таблицы `settings`
--

CREATE TABLE `settings` (
  `db_version` text NOT NULL,
  `last_update_date` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `settings`
--

INSERT INTO `settings` (`db_version`, `last_update_date`) VALUES
('0.1', '2017-01-19 14:46:50'),
('0.2', '2017-01-20 14:28:29');

-- --------------------------------------------------------

--
-- Структура таблицы `user_test`
--

CREATE TABLE `user_test` (
  `id_user_test` int(11) NOT NULL,
  `date_test` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `result` int(11) NOT NULL,
  `udid` text NOT NULL,
  `id_hostel` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Дамп данных таблицы `user_test`
--

INSERT INTO `user_test` (`id_user_test`, `date_test`, `result`, `udid`, `id_hostel`) VALUES
(1, '2017-01-20 17:59:13', 0, 'FFFFFFFFE90D0A945AFD4647977F46DF263B234E', 0),
(2, '2017-01-20 18:09:29', 1, 'FFFFFFFFE90D0A945AFD4647977F46DF263B234E', 1);

--
-- Индексы сохранённых таблиц
--

--
-- Индексы таблицы `hostel`
--
ALTER TABLE `hostel`
  ADD PRIMARY KEY (`id_hostel`);

--
-- Индексы таблицы `hostel2metro`
--
ALTER TABLE `hostel2metro`
  ADD PRIMARY KEY (`id_hostel2metro`);

--
-- Индексы таблицы `hostel2phone`
--
ALTER TABLE `hostel2phone`
  ADD PRIMARY KEY (`id_hostel2phone`);

--
-- Индексы таблицы `metro`
--
ALTER TABLE `metro`
  ADD PRIMARY KEY (`id_metro`);

--
-- Индексы таблицы `phone`
--
ALTER TABLE `phone`
  ADD PRIMARY KEY (`id_phone`);

--
-- Индексы таблицы `user_test`
--
ALTER TABLE `user_test`
  ADD PRIMARY KEY (`id_user_test`);

--
-- AUTO_INCREMENT для сохранённых таблиц
--

--
-- AUTO_INCREMENT для таблицы `hostel`
--
ALTER TABLE `hostel`
  MODIFY `id_hostel` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT для таблицы `hostel2metro`
--
ALTER TABLE `hostel2metro`
  MODIFY `id_hostel2metro` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT для таблицы `hostel2phone`
--
ALTER TABLE `hostel2phone`
  MODIFY `id_hostel2phone` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT для таблицы `metro`
--
ALTER TABLE `metro`
  MODIFY `id_metro` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;
--
-- AUTO_INCREMENT для таблицы `phone`
--
ALTER TABLE `phone`
  MODIFY `id_phone` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;
--
-- AUTO_INCREMENT для таблицы `user_test`
--
ALTER TABLE `user_test`
  MODIFY `id_user_test` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

  CREATE USER if not exists 'user'@'localhost' IDENTIFIED BY 'user';
GRANT ALL PRIVILEGES ON hostel.*		TO 'user'@'localhost' WITH GRANT OPTION;