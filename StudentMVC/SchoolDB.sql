USE [master]

IF (EXISTS (
    SELECT NAME 
    FROM master.dbo.sysdatabases 
    WHERE ('[' + NAME + ']' = N'SchoolDB' OR NAME = N'SchoolDB')
))
DROP DATABASE SchoolDB
GO

CREATE DATABASE SchoolDB
GO

USE SchoolDB
GO


/* =========================================================
   TEACHER TABLE
   ========================================================= */

CREATE TABLE Teacher
(
    TeacherId INT CONSTRAINT pk_TID PRIMARY KEY IDENTITY(1,1),
    TeacherName VARCHAR(20) NOT NULL
)
GO


/* =========================================================
   STUDENT TABLE
   ========================================================= */

CREATE TABLE Student
(
    StudentId INT CONSTRAINT pk_SID PRIMARY KEY IDENTITY(100,1),
    StudentName VARCHAR(20) NOT NULL
)
GO


/* =========================================================
   CLASS TABLE
   ========================================================= */

CREATE TABLE Class 
(
    ClassId INT CONSTRAINT pk_CID PRIMARY KEY IDENTITY(200,1),
    ClassName VARCHAR(20) NOT NULL, 
    TeacherId INT CONSTRAINT fk_TID 
        FOREIGN KEY REFERENCES Teacher(TeacherId)
)
GO


/* =========================================================
   MARKS TABLE
   ========================================================= */

CREATE TABLE Marks
(
    MarksId INT CONSTRAINT pk_MID PRIMARY KEY IDENTITY(300,1),
    Marks INT NOT NULL 
        CONSTRAINT ck_Marks CHECK(Marks <= 100),
    StudentId INT 
        CONSTRAINT fk_SID FOREIGN KEY REFERENCES Student(StudentId),
    ClassId INT 
        CONSTRAINT fk_CID FOREIGN KEY REFERENCES Class(ClassId)
)
GO


/* =========================================================
   INSERT TEACHERS
   ========================================================= */

INSERT INTO Teacher (TeacherName)
VALUES
('Rohith'),
('Poorna'),
('Ajay'),
('Vikram'),
('Saleem'),
('Kiran'),
('Meena'),
('Ramesh'),
('Suresh'),
('Divya')
GO


/* =========================================================
   INSERT STUDENTS
   ========================================================= */

INSERT INTO Student (StudentName)
VALUES
('Hari'),
('Akash'),
('Venky'),
('Anji'),
('Srinivas'),
('Naveen'),
('Rahul'),
('Priya'),
('Karthik'),
('Swathi'),
('Manoj'),
('Deepika'),
('Arjun'),
('Sneha'),
('Tarun'),
('Varun')
GO


/* =========================================================
   INSERT CLASSES
   ========================================================= */

INSERT INTO Class (ClassName, TeacherId)
VALUES
('Java', 1),
('DBMS', 2),
('MSSQL', 2),
('CSharp', 3),
('Angular', 4),
('React', 4),
('Python', 5),
('HTML', 6),
('CSS', 6),
('JavaScript', 7),
('Spring', 7),
('Azure', 8),
('AWS', 8),
('PowerBI', 9),
('Python Advanced', 9),
('DataScience', 10),
('MachineLearning', 10)
GO


/* =========================================================
   INSERT MARKS
   ========================================================= */

INSERT INTO Marks (Marks, StudentId, ClassId)
VALUES

/* ---------------------------------------------------------
   Hari - 100
   --------------------------------------------------------- */

(65,100,200),
(58,100,201),
(62,100,202),

/* ---------------------------------------------------------
   Akash - 101
   --------------------------------------------------------- */

(65,101,202),
(68,101,203),

/* ---------------------------------------------------------
   Venky - 102
   --------------------------------------------------------- */

(62,102,204),
(66,102,205),
(62,102,206),

/* ---------------------------------------------------------
   Anji - 103
   --------------------------------------------------------- */

(73,103,205),
(64,103,200),

/* ---------------------------------------------------------
   Srinivas - 104
   --------------------------------------------------------- */

(69,104,201),
(61,104,203),
(60,104,205),
(65,104,202),
(63,104,206),

/* ---------------------------------------------------------
   Naveen - 105
   --------------------------------------------------------- */

(95,105,200),
(88,105,201),
(76,105,202),
(91,105,203),
(84,105,204),

/* ---------------------------------------------------------
   Rahul - 106
   --------------------------------------------------------- */

(45,106,200),
(55,106,201),
(65,106,202),
(72,106,203),
(39,106,204),

/* ---------------------------------------------------------
   Priya - 107
   --------------------------------------------------------- */

(100,107,200),
(92,107,201),
(89,107,202),
(96,107,205),
(85,107,206),

/* ---------------------------------------------------------
   Karthik - 108
   --------------------------------------------------------- */

(40,108,200),
(41,108,201),
(59,108,202),
(60,108,203),
(64,108,204),

/* ---------------------------------------------------------
   Swathi - 109
   --------------------------------------------------------- */

(78,109,200),
(82,109,201),
(88,109,202),
(90,109,203),
(95,109,204),

/* ---------------------------------------------------------
   Manoj - 110
   --------------------------------------------------------- */

(35,110,200),
(38,110,201),
(42,110,202),
(55,110,205),
(61,110,206),

/* ---------------------------------------------------------
   Deepika - 111
   --------------------------------------------------------- */

(65,111,207),
(75,111,208),
(85,111,209),
(95,111,210),

/* ---------------------------------------------------------
   Arjun - 112
   --------------------------------------------------------- */

(50,112,207),
(60,112,208),
(70,112,209),
(80,112,210),

/* ---------------------------------------------------------
   Sneha - 113
   --------------------------------------------------------- */

(90,113,211),
(92,113,212),
(88,113,213),
(95,113,214),

/* ---------------------------------------------------------
   Tarun - 114
   --------------------------------------------------------- */

(30,114,211),
(45,114,212),
(55,114,213),
(65,114,214)

/*
   Varun - 115

   Intentionally has NO MARKS.
   Used for LEFT JOIN testing.
*/

GO


/* =========================================================
   FUNCTION 1
   GET CLASSES FOR A TEACHER
   ========================================================= */

CREATE FUNCTION ufn_TeacherClass
(
    @TeacherName VARCHAR(20)
)
RETURNS TABLE
AS
RETURN 
(
    SELECT 
        c.ClassName,
        t.TeacherName
    FROM Teacher t
    INNER JOIN Class c 
        ON c.TeacherId = t.TeacherId
    WHERE t.TeacherName = @TeacherName
);
GO


SELECT * 
FROM ufn_TeacherClass('Poorna');
GO


/* =========================================================
   FUNCTION 2
   NUMBER OF STUDENTS IN A CLASS
   ========================================================= */

CREATE FUNCTION ufn_NoOfStudentClass
(
    @ClassName VARCHAR(20)
)
RETURNS INT
AS
BEGIN

    DECLARE @numStudent INT;

    SELECT @numStudent = COUNT(DISTINCT m.StudentId)
    FROM Class c
    INNER JOIN Marks m 
        ON c.ClassId = m.ClassId
    WHERE c.ClassName = @ClassName;

    RETURN @numStudent;

END;
GO


DECLARE @ClassName VARCHAR(20) = 'React';

SELECT dbo.ufn_NoOfStudentClass(@ClassName) 
AS NoOfStudentsInClass;

GO


/* =========================================================
   FUNCTION 3
   NUMBER OF CLASSES ENROLLED BY STUDENT
   ========================================================= */

CREATE FUNCTION ufn_EnrolledStudentClass
(
    @studentName VARCHAR(20)
)
RETURNS INT
AS
BEGIN

    DECLARE @numClasses INT;

    SELECT @numClasses = COUNT(DISTINCT m.ClassId)
    FROM Marks m
    WHERE m.StudentId = 
    (
        SELECT StudentId 
        FROM Student 
        WHERE StudentName = @studentName
    );

    RETURN @numClasses;

END;
GO


DECLARE @studentName VARCHAR(20) = 'Akash';

SELECT dbo.ufn_EnrolledStudentClass(@studentName) 
AS NoOfClassesEnrolledByStudent;

GO


/* =========================================================
   FUNCTION 4
   PASS / FAIL COUNT BY CLASS
   ========================================================= */

CREATE FUNCTION dbo.PassFailCountByClass()
RETURNS TABLE
AS
RETURN
(
    SELECT 
        c.ClassName,

        SUM(
            CASE 
                WHEN m.Marks >= 65 THEN 1 
                ELSE 0 
            END
        ) AS Passed,

        SUM(
            CASE 
                WHEN m.Marks < 65 THEN 1 
                ELSE 0 
            END
        ) AS Failed

    FROM Class c
    LEFT JOIN Marks m 
        ON c.ClassId = m.ClassId

    GROUP BY c.ClassName
);
GO


SELECT * 
FROM dbo.PassFailCountByClass();

GO


/* =========================================================
   STORED PROCEDURE
   STUDENT SCORECARD
   ========================================================= */

CREATE PROCEDURE usp_Scorecard
(
    @StudentName VARCHAR(20)
)
AS
BEGIN

    SELECT * 
    INTO #ScoreC 
    FROM
    (
        SELECT DISTINCT 
            S.StudentName,
            C.ClassName,
            M.Marks

        FROM Class C

        INNER JOIN Marks M 
            ON C.ClassId = M.ClassId

        INNER JOIN Student S 
            ON M.StudentId = S.StudentId

        WHERE S.StudentName = @StudentName

        GROUP BY 
            S.StudentName,
            C.ClassName,
            M.Marks

    ) AS P

    PIVOT
    (
        SUM(Marks)

        FOR [ClassName] IN
        (
            [Java],
            [DBMS],
            [MSSQL],
            [CSharp],
            [Angular],
            [React],
            [Python],
            [HTML],
            [CSS],
            [JavaScript],
            [Spring],
            [Azure],
            [AWS],
            [PowerBI],
            [Python Advanced],
            [DataScience],
            [MachineLearning]
        )

    ) AS Pvt;


    SELECT 
        S.StudentName,

        ISNULL(Java,0) AS Java,
        ISNULL(DBMS,0) AS DBMS,
        ISNULL(MSSQL,0) AS MSSQL,
        ISNULL(CSharp,0) AS CSharp,
        ISNULL(Angular,0) AS Angular,
        ISNULL(React,0) AS React,
        ISNULL(Python,0) AS Python,
        ISNULL(HTML,0) AS HTML,
        ISNULL(CSS,0) AS CSS,
        ISNULL(JavaScript,0) AS JavaScript,
        ISNULL(Spring,0) AS Spring,
        ISNULL(Azure,0) AS Azure,
        ISNULL(AWS,0) AS AWS,
        ISNULL(PowerBI,0) AS PowerBI,
        ISNULL([Python Advanced],0) AS [Python Advanced],
        ISNULL(DataScience,0) AS DataScience,
        ISNULL(MachineLearning,0) AS MachineLearning,

        CASE
            WHEN AVG(Marks) >= 90 THEN 'A+'
            WHEN AVG(Marks) >= 80 THEN 'A'
            WHEN AVG(Marks) >= 70 THEN 'B+'
            WHEN AVG(Marks) >= 60 THEN 'B'
            WHEN AVG(Marks) >= 40 THEN 'C'
            ELSE 'F'
        END AS Grade

    FROM #ScoreC A

    INNER JOIN Student S 
        ON S.StudentName = A.StudentName

    INNER JOIN Marks M 
        ON M.StudentId = S.StudentId

    GROUP BY 
        S.StudentName,
        Java,
        DBMS,
        MSSQL,
        CSharp,
        Angular,
        React,
        Python,
        HTML,
        CSS,
        JavaScript,
        Spring,
        Azure,
        AWS,
        PowerBI,
        [Python Advanced],
        DataScience,
        MachineLearning;

END;
GO


/* =========================================================
   TEST SCORECARD
   ========================================================= */

EXEC usp_Scorecard 'Naveen';
GO

EXEC usp_Scorecard 'Rahul';
GO

EXEC usp_Scorecard 'Priya';
GO

EXEC usp_Scorecard 'Deepika';
GO

EXEC usp_Scorecard 'Sneha';
GO


/* =========================================================
   SOFT DELETE COLUMNS
   ========================================================= */

ALTER TABLE Student
ADD Deleted BIT DEFAULT 0;
GO

ALTER TABLE Marks
ADD Deleted BIT DEFAULT 0;
GO


/* =========================================================
   DELETE STUDENT PROCEDURE
   ========================================================= */

CREATE PROCEDURE sp_DeleteStudent
(
    @StudentId INT
)
AS
BEGIN

    IF NOT EXISTS
    (
        SELECT *
        FROM Student
        WHERE StudentId = @StudentId
    )
        RETURN 0;


    UPDATE Student
    SET Deleted = 1
    WHERE StudentId = @StudentId;


    UPDATE Marks
    SET Deleted = 1
    WHERE StudentId IN
    (
        SELECT StudentId
        FROM Student
        WHERE StudentId = @StudentId
        AND Deleted = 1
    );


    DECLARE @StudentName VARCHAR(20);


    SELECT @StudentName = StudentName
    FROM Student
    WHERE StudentId = @StudentId;


    PRINT 'The student ' + @StudentName + ' has been deleted.';


    SELECT *
    FROM Student
    WHERE StudentId = @StudentId;

END;
GO


/* =========================================================
   DELETED STUDENT FUNCTION
   ========================================================= */

CREATE FUNCTION DeletedStudent
(
    @StudentName VARCHAR(20)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        S.StudentName,
        M.Marks,
        C.ClassName

    FROM Student S

    INNER JOIN Marks M 
        ON S.StudentId = M.StudentId

    INNER JOIN Class C 
        ON C.ClassId = M.ClassId

    WHERE S.StudentName = @StudentName
    AND S.Deleted = 1
);
GO


/* =========================================================
   TEST SOFT DELETE
   Naveen = StudentId 105
   ========================================================= */

EXEC sp_DeleteStudent 105;
GO


/* =========================================================
   CHECK DELETED STUDENTS
   ========================================================= */

SELECT *
FROM Student
WHERE Deleted = 1;
GO


/* =========================================================
   CHECK DELETED MARKS
   ========================================================= */

SELECT *
FROM Marks
WHERE Deleted = 1;
GO


/* =========================================================
   CHECK DELETED STUDENT FUNCTION
   ========================================================= */

SELECT *
FROM DeletedStudent('Naveen');
GO


/* =========================================================
   TEST STUDENT WITH NO MARKS
   Varun = StudentId 115
   ========================================================= */

SELECT 
    S.StudentId,
    S.StudentName,
    M.MarksId,
    M.Marks

FROM Student S

LEFT JOIN Marks M
    ON S.StudentId = M.StudentId

WHERE S.StudentId = 115;
GO