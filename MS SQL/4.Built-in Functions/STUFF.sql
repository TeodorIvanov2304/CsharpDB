SELECT STUFF('This is a pleaceholder for p',28,1,'inserted')

SELECT STUFF('This is a pleaceholder',
  CHARINDEX('pleaceholder','This is a pleaceholder'),
        LEN('pleaceholder'),'template')