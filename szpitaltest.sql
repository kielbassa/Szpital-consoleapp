-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Nov 05, 2023 at 10:39 PM
-- Server version: 10.4.28-MariaDB
-- PHP Version: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `szpitaltest`
--

-- --------------------------------------------------------

--
-- Table structure for table `kolejka`
--

CREATE TABLE `kolejka` (
  `wizyta_id` int(11) NOT NULL,
  `pacjent_id` int(11) NOT NULL,
  `lekarz_id` int(11) NOT NULL,
  `oddzial_id` int(11) NOT NULL,
  `date` date NOT NULL,
  `begin` char(5) NOT NULL,
  `end` char(5) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Dumping data for table `kolejka`
--

INSERT INTO `kolejka` (`wizyta_id`, `pacjent_id`, `lekarz_id`, `oddzial_id`, `date`, `begin`, `end`) VALUES
(1, 6, 3, 1, '2023-11-17', '09:00', '10:00');

-- --------------------------------------------------------

--
-- Table structure for table `lokalizacje`
--

CREATE TABLE `lokalizacje` (
  `location_id` int(11) NOT NULL,
  `miasto` char(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Dumping data for table `lokalizacje`
--

INSERT INTO `lokalizacje` (`location_id`, `miasto`) VALUES
(1, 'Gdańsk'),
(2, 'Gdynia'),
(3, 'Warszawa'),
(4, 'Kraków'),
(5, 'Poznań'),
(6, 'Sosnowiec');

-- --------------------------------------------------------

--
-- Table structure for table `oddzialy`
--

CREATE TABLE `oddzialy` (
  `oddzial_id` int(11) NOT NULL,
  `szpital_id` int(11) NOT NULL,
  `oddzial_type` enum('Neurologia','Kardiologia','Peditria','Ortopedia','Okulistyka','Psychologia') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Dumping data for table `oddzialy`
--

INSERT INTO `oddzialy` (`oddzial_id`, `szpital_id`, `oddzial_type`) VALUES
(1, 1, 'Kardiologia'),
(2, 1, 'Neurologia'),
(3, 1, 'Peditria'),
(4, 1, 'Ortopedia'),
(5, 1, 'Okulistyka'),
(6, 2, 'Kardiologia'),
(7, 3, 'Ortopedia'),
(8, 4, 'Okulistyka'),
(9, 5, 'Kardiologia'),
(10, 6, 'Kardiologia'),
(11, 3, 'Neurologia'),
(12, 4, 'Neurologia'),
(13, 3, 'Okulistyka'),
(14, 7, 'Neurologia'),
(15, 5, 'Psychologia');

-- --------------------------------------------------------

--
-- Table structure for table `pracownicy`
--

CREATE TABLE `pracownicy` (
  `lekarz_id` int(11) NOT NULL,
  `oddzial_id` int(11) NOT NULL,
  `pon` char(5) DEFAULT NULL,
  `wt` char(5) DEFAULT NULL,
  `sr` char(5) DEFAULT NULL,
  `czw` char(5) DEFAULT NULL,
  `pt` char(5) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Dumping data for table `pracownicy`
--

INSERT INTO `pracownicy` (`lekarz_id`, `oddzial_id`, `pon`, `wt`, `sr`, `czw`, `pt`) VALUES
(2, 2, '10-15', '10-15', '15-18', NULL, NULL),
(3, 1, '14-18', NULL, '9-14', NULL, '8-10'),
(4, 1, NULL, '8-14', '10-14', '15-18', NULL),
(5, 2, '8-13', '13-16', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `szpitale`
--

CREATE TABLE `szpitale` (
  `szpital_id` int(11) NOT NULL,
  `name` char(50) NOT NULL,
  `location` enum('Gdańsk','Gdynia','Warszawa','Kraków','Poznań','Sosnowiec') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_polish_ci;

--
-- Dumping data for table `szpitale`
--

INSERT INTO `szpitale` (`szpital_id`, `name`, `location`) VALUES
(1, 'Szpital Miejski w Gdańsku', 'Gdańsk'),
(2, 'Szpital Wojskowy w Warszawie', 'Warszawa'),
(3, 'Szpital Miejski w Sosnowcu', 'Sosnowiec'),
(4, 'Szpital Wojskowy w Gdańsku', 'Gdańsk'),
(5, 'Szpital Miejski w Gdyni', 'Gdynia'),
(6, 'Szpital Wojskowy w Warszawie', 'Warszawa'),
(7, 'Szpital Uniwersytecki w Gdańsku', 'Gdańsk'),
(8, 'Szpital Wojskowy w Gdyni', 'Gdynia'),
(9, 'Szpital Miejski w Krakowie', 'Kraków'),
(10, 'Szpital Miejski w Poznaniu', 'Poznań'),
(11, 'Szpital Wojskowy w Krakowie', 'Kraków');

-- --------------------------------------------------------

--
-- Table structure for table `uzytkownicy`
--

CREATE TABLE `uzytkownicy` (
  `user_id` int(11) NOT NULL,
  `login` varchar(30) NOT NULL,
  `haslo` varchar(100) NOT NULL,
  `pin` varchar(4) NOT NULL,
  `user_type` enum('USER','ADMIN','LEKARZ') NOT NULL,
  `imie` char(30) NOT NULL,
  `nazwisko` char(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `uzytkownicy`
--

INSERT INTO `uzytkownicy` (`user_id`, `login`, `haslo`, `pin`, `user_type`, `imie`, `nazwisko`) VALUES
(1, 'test', 'test123', '1234', 'ADMIN', 'Adam', 'Adminowski'),
(2, 'hejka', 'tulenka', '4321', 'LEKARZ', 'Halina', 'Ejkowska'),
(3, 'dr_oetker', 'oetker123', '0123', 'LEKARZ', 'August', 'Oetker'),
(4, 'leszke', 'zimnylechwwannie', '1111', 'LEKARZ', 'Leszek', 'Lekarski'),
(5, 'gianni', 'gralemwshalke', '1234', 'LEKARZ', 'Tomasz', 'Hajto'),
(6, 'cczarek', 'avecezar', '1234', 'USER', 'Cezary', 'Cezary'),
(7, 'scolvus', 'jestemprawdziwy', '1492', 'USER', 'Jan', 'Z Kolna');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `kolejka`
--
ALTER TABLE `kolejka`
  ADD PRIMARY KEY (`wizyta_id`),
  ADD KEY `wizyta_id` (`wizyta_id`);

--
-- Indexes for table `lokalizacje`
--
ALTER TABLE `lokalizacje`
  ADD PRIMARY KEY (`location_id`),
  ADD UNIQUE KEY `location_id` (`location_id`);

--
-- Indexes for table `oddzialy`
--
ALTER TABLE `oddzialy`
  ADD PRIMARY KEY (`oddzial_id`);

--
-- Indexes for table `pracownicy`
--
ALTER TABLE `pracownicy`
  ADD PRIMARY KEY (`lekarz_id`) USING BTREE;

--
-- Indexes for table `szpitale`
--
ALTER TABLE `szpitale`
  ADD PRIMARY KEY (`szpital_id`),
  ADD UNIQUE KEY `szpital_id` (`szpital_id`,`name`);

--
-- Indexes for table `uzytkownicy`
--
ALTER TABLE `uzytkownicy`
  ADD PRIMARY KEY (`user_id`),
  ADD KEY `user_id` (`user_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `uzytkownicy`
--
ALTER TABLE `uzytkownicy`
  MODIFY `user_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
