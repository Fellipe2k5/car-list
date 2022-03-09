CREATE TABLE `cars` (
  `id` INT(10) AUTO_INCREMENT PRIMARY KEY,
  `make` varchar(50) NULL,
  `model` varchar(50) NULL,
  `color` varchar(20) NULL,
  `plate` varchar(6) NULL,
  `date_make` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
