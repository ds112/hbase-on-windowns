# Mở file
file = open("putTacGia.txt", "w")
for num in range(1,50001):
    s2=str(num)
    s3="put 'TACGIA','TG" + s2 + "','TG:MSTG','MSTG" + s2 + "'\n"
    s4="put 'TACGIA','TG" + s2 + "','TG:TENTG','TENTG" + s2 + "'\n"
    s5="put 'TACGIA','TG" + s2 + "','TG:SDT','SDT" + s2 + "'\n"
    s6="put 'TACGIA','TG" + s2 + "','TG:EMAIL','EMAIL" + s2 + "@DS112.COM'\n"
    file.write(s3)
    file.write(s4)
    file.write(s5)
    file.write(s6)
    pass
# Đóng file
file.close()