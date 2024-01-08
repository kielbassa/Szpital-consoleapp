-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Sty 07, 2024 at 04:30 PM
-- Wersja serwera: 10.4.32-MariaDB
-- Wersja PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `test3`
--

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `admins`
--

CREATE TABLE `admins` (
  `ID` int(11) NOT NULL,
  `login` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `admins`
--

INSERT INTO `admins` (`ID`, `login`, `password`) VALUES
(1, 'a', 'a');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `doctors`
--

CREATE TABLE `doctors` (
  `ID` int(11) NOT NULL,
  `login` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL,
  `firstName` varchar(100) DEFAULT NULL,
  `lastName` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `doctors`
--

INSERT INTO `doctors` (`ID`, `login`, `password`, `firstName`, `lastName`) VALUES
(1, 'doc_user_123', 'pass_abc_123', 'Adam', 'Nowak'),
(2, 'dr_eva_kow', 'pass1234', 'Ewa', 'Kowalska'),
(3, 'm_wisniewski', 'myPass567', 'Michał', 'Wiśniewski'),
(4, 'alicja_doc', 'aPass#321', 'Alicja', 'Dąbrowska'),
(5, 'p_lisowski', 'securePass', 'Piotr', 'Lisowski'),
(6, 'zofia_j', 'zofiaPass456', 'Zofia', 'Jankowska'),
(7, 'k_kaczmarek', 'kkrules789', 'Krzysztof', 'Kaczmarek'),
(8, 'a_szymanska', 'asPass_2022', 'Anna', 'Szymańska'),
(9, 'bartekW', 'bW_pass_123', 'Bartosz', 'Wójcik'),
(10, 'nat_pawlak', 'natPass!567', 'Natalia', 'Pawlak'),
(11, 'dr_marcin_k', 'pass4Marcin', 'Marcin', 'Kowalczyk'),
(12, 'kingaZ', 'kZPass#22', 'Kinga', 'Zielińska'),
(13, 'g_wozniak', 'gwozPass789', 'Grzegorz', 'Woźniak'),
(14, 'dom_kaczor', 'domPass123', 'Dominika', 'Kaczor'),
(15, 'kamil_o', 'kamilPass_456', 'Kamil', 'Olszewski'),
(16, 'a_sawicka', 'aSaw_pass_789', 'Aleksandra', 'Sawicka');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `hospitals`
--

CREATE TABLE `hospitals` (
  `ID` int(11) NOT NULL,
  `name` varchar(100) DEFAULT NULL,
  `location` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `hospitals`
--

INSERT INTO `hospitals` (`ID`, `name`, `location`) VALUES
(1, 'Szpital Wojskowy', 'Warszawa'),
(2, 'Szpital Miejski', 'Kraków'),
(3, 'Szpital Kliniczny', 'Łódź'),
(4, 'Szpital Miejski', 'Wrocław'),
(5, 'Szpital Regionalny', 'Poznań'),
(6, 'Szpital Dziecięcy', 'Warszawa'),
(7, 'Szpital Wojewódzki', 'Kraków'),
(8, 'Szpital Wojewódzki', 'Łódź');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `information`
--

CREATE TABLE `information` (
  `ID` int(11) NOT NULL,
  `HospitalID` int(11) DEFAULT NULL,
  `WardID` int(11) DEFAULT NULL,
  `DoctorID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `information`
--

INSERT INTO `information` (`ID`, `HospitalID`, `WardID`, `DoctorID`) VALUES
(1, 1, 2, 16),
(2, 1, 1, 15),
(3, 2, 3, 14),
(4, 2, 4, 13),
(5, 3, 5, 12),
(6, 3, 4, 11),
(7, 4, 3, 10),
(8, 4, 5, 9),
(9, 5, 3, 8),
(10, 5, 5, 7),
(11, 6, 1, 6),
(12, 6, 2, 5),
(13, 7, 5, 4),
(14, 7, 2, 3),
(15, 8, 4, 2),
(16, 8, 1, 1);

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `patients`
--

CREATE TABLE `patients` (
  `ID` int(11) NOT NULL,
  `login` varchar(100) DEFAULT NULL,
  `password` varchar(100) DEFAULT NULL,
  `PIN` varchar(4) DEFAULT NULL,
  `firstName` varchar(100) DEFAULT NULL,
  `lastName` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `patients`
--

INSERT INTO `patients` (`ID`, `login`, `password`, `PIN`, `firstName`, `lastName`) VALUES
(1, 'miska', 'hashas', '1234', 'Anna', 'Kowalska'),
(2, 'jachu', 'haslo123', '5678', 'Jan', 'Nowak'),
(3, 'animation', 'nie', '9876', 'Ewa', 'Wiśniewska'),
(4, 'piotr', '12345', '5432', 'Piotr', 'Dąbrowski'),
(5, 'kasix', 'hej', '8765', 'Katarzyna', 'Lisowska'),
(6, 'a', 'a', '1234', 'Michał', 'Lis'),
(7, 'z', 'z', '1234', 'z', 'z'),
(8, 'k', 'k', '1234', 'k', 'k'),
(9, 'h', 'h', '1234', '', ''),
(10, 'marek', 'marek123', '1234', 'Marek', 'Markowski');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `visits`
--

CREATE TABLE `visits` (
  `ID` int(11) NOT NULL,
  `PatientID` int(11) NOT NULL,
  `InformationID` int(11) DEFAULT NULL,
  `VisitDate` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `visits`
--

INSERT INTO `visits` (`ID`, `PatientID`, `InformationID`, `VisitDate`) VALUES
(1, 2, 5, '2024-02-06'),
(2, 3, 2, '2024-02-12'),
(3, 5, 1, '2024-02-18'),
(9, 3, 5, '2023-12-12'),
(10, 6, 9, '2023-02-23'),
(11, 2, 1, '2024-02-12'),
(12, 4, 1, '2024-12-12'),
(15, 6, 3, '2024-01-01'),
(16, 6, 3, '2024-12-12'),
(17, 2, 3, '2024-05-05'),
(18, 2, 10, '2024-02-18'),
(19, 6, 5, '2023-10-10'),
(20, 6, 1, '2024-03-14');

-- --------------------------------------------------------

--
-- Struktura tabeli dla tabeli `wards`
--

CREATE TABLE `wards` (
  `ID` int(11) NOT NULL,
  `name` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_polish_ci;

--
-- Dumping data for table `wards`
--

INSERT INTO `wards` (`ID`, `name`) VALUES
(1, 'Chirurgia'),
(2, 'Pediatria'),
(3, 'Neurologia'),
(4, 'Ginekologia'),
(5, 'Kardiologia');

--
-- Indeksy dla zrzutów tabel
--

--
-- Indeksy dla tabeli `admins`
--
ALTER TABLE `admins`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `doctors`
--
ALTER TABLE `doctors`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `hospitals`
--
ALTER TABLE `hospitals`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `information`
--
ALTER TABLE `information`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `HospitalID` (`HospitalID`),
  ADD KEY `WardID` (`WardID`),
  ADD KEY `DoctorID` (`DoctorID`);

--
-- Indeksy dla tabeli `patients`
--
ALTER TABLE `patients`
  ADD PRIMARY KEY (`ID`);

--
-- Indeksy dla tabeli `visits`
--
ALTER TABLE `visits`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `InformationID` (`InformationID`);

--
-- Indeksy dla tabeli `wards`
--
ALTER TABLE `wards`
  ADD PRIMARY KEY (`ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `admins`
--
ALTER TABLE `admins`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `doctors`
--
ALTER TABLE `doctors`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `hospitals`
--
ALTER TABLE `hospitals`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `information`
--
ALTER TABLE `information`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `patients`
--
ALTER TABLE `patients`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `visits`
--
ALTER TABLE `visits`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `wards`
--
ALTER TABLE `wards`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `information`
--
ALTER TABLE `information`
  ADD CONSTRAINT `information_ibfk_1` FOREIGN KEY (`HospitalID`) REFERENCES `hospitals` (`ID`),
  ADD CONSTRAINT `information_ibfk_2` FOREIGN KEY (`WardID`) REFERENCES `wards` (`ID`),
  ADD CONSTRAINT `information_ibfk_3` FOREIGN KEY (`DoctorID`) REFERENCES `doctors` (`ID`);

--
-- Constraints for table `visits`
--
ALTER TABLE `visits`
  ADD CONSTRAINT `visits_ibfk_1` FOREIGN KEY (`InformationID`) REFERENCES `information` (`ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
