use ClockStoreDB
go

SELECT Email, COUNT(*)
FROM AspNetUsers
GROUP BY Email
HAVING COUNT(*) > 1;

-- Tìm các ID của người dùng trùng lặp (giữ lại một bản ghi)
SELECT Id, Email
FROM AspNetUsers
WHERE Email IN (
    SELECT Email
    FROM AspNetUsers
    GROUP BY Email
    HAVING COUNT(*) > 1
)
ORDER BY Email, Id;

-- Xóa các bản ghi trùng lặp, giữ lại bản ghi có ID nhỏ nhất (hoặc bất kỳ tiêu chí nào bạn muốn)
DELETE FROM AspNetUsers
WHERE Id NOT IN (
    SELECT MIN(Id)
    FROM AspNetUsers
    GROUP BY Email
)
AND Email IN (
    SELECT Email
    FROM AspNetUsers
    GROUP BY Email
    HAVING COUNT(*) > 1
);