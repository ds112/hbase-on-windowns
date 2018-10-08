# Mở file
file = open("putSach.txt", "w")
for num in range(1,50001):
    s2=str(num)
    s3="put 'SACH','SA" + s2 + "','SA:MSS','MSS" + s2 + "'\n"
    s4="put 'SACH','SA" + s2 + "','SA:TENSACH','TENSACH" + s2 + "'\n"
    s5="put 'SACH','SA" + s2 + "','SA:SOTRANG','SOTRANG" + s2 + "'\n"
    s6="put 'SACH','SA" + s2 + "','SA:ST','ST" + s2 + "'\n"
    s7="put 'SACH','SA" + s2 + "','SA:MSNXB','MSNXB" + s2 + "'\n"
    file.write(s3)
    file.write(s4)
    file.write(s5)
    file.write(s6)
    file.write(s7)
    pass
# Đóng file
file.close()