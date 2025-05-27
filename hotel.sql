-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versione server:              9.0.1 - MySQL Community Server - GPL
-- S.O. server:                  Win64
-- HeidiSQL Versione:            12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Dump della struttura del database hotel
CREATE DATABASE IF NOT EXISTS `hotel` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `hotel`;

-- Dump della struttura di tabella hotel.camere
CREATE TABLE IF NOT EXISTS `camere` (
  `id` int NOT NULL,
  `costo` double DEFAULT NULL,
  `tipo` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella hotel.camere: ~9 rows (circa)
INSERT INTO `camere` (`id`, `costo`, `tipo`) VALUES
	(101, 50, 'Singola'),
	(102, 50, 'Singola'),
	(103, 50, 'Singola'),
	(201, 70, 'Doppia std'),
	(202, 70, 'Doppia std'),
	(203, 90, 'Doppia Deluxe'),
	(301, 100, 'Tripla std'),
	(302, 100, 'Tripla std'),
	(303, 120, 'Tripla Deluxe');

-- Dump della struttura di tabella hotel.dettagli_prenotazioni
CREATE TABLE IF NOT EXISTS `dettagli_prenotazioni` (
  `idPrenotazione` int NOT NULL,
  `idCamera` int NOT NULL,
  PRIMARY KEY (`idPrenotazione`,`idCamera`),
  KEY `FK_dettagli_prenotazioni_camere` (`idCamera`),
  CONSTRAINT `FK_dettagli_prenotazioni_camere` FOREIGN KEY (`idCamera`) REFERENCES `camere` (`id`),
  CONSTRAINT `FK_dettagli_prenotazioni_prenotazioni` FOREIGN KEY (`idPrenotazione`) REFERENCES `prenotazioni` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella hotel.dettagli_prenotazioni: ~9 rows (circa)
INSERT INTO `dettagli_prenotazioni` (`idPrenotazione`, `idCamera`) VALUES
	(2, 101),
	(3, 101),
	(20, 101),
	(20, 102),
	(21, 103),
	(2, 201),
	(21, 201),
	(22, 202),
	(22, 203),
	(1, 301),
	(3, 303);

-- Dump della struttura di tabella hotel.prenotazioni
CREATE TABLE IF NOT EXISTS `prenotazioni` (
  `id` int NOT NULL AUTO_INCREMENT,
  `nomeCliente` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `cognomeCliente` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci DEFAULT NULL,
  `sesso` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `dataCheckIn` date DEFAULT NULL,
  `dataCheckOut` date DEFAULT NULL,
  `numPersone` int DEFAULT NULL,
  `telefono` varchar(50) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Dump dei dati della tabella hotel.prenotazioni: ~6 rows (circa)
INSERT INTO `prenotazioni` (`id`, `nomeCliente`, `cognomeCliente`, `sesso`, `dataCheckIn`, `dataCheckOut`, `numPersone`, `telefono`) VALUES
	(1, 'Mario', 'Verdi', 'm', '2024-12-18', '2024-12-23', 3, '312-435-6666'),
	(2, 'Anna', 'Rosa', 'f', '2025-01-10', '2025-01-21', 1, '213-3422-111'),
	(3, 'Giorgia', 'Grigio', 'f', '2025-03-12', '2025-03-14', 3, '034243232'),
	(20, 'Andrea', 'Adidas', 'm', '2025-02-12', '2025-02-14', 2, '231231'),
	(21, 'AA', 'AAA', 'm', '2025-02-13', '2025-02-20', 3, '4444'),
	(22, 'A', 'iiii', 'm', '2025-02-12', '2025-02-19', 3, '444');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
