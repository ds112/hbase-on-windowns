# Mở file
import random
file = open("dataTacGiaSach.txt", "w")
for num in range(1,50001):
    s2=str(num)
    #s3="insert into sach (masach,tensach,sotrang,manxb) values('masach"+str(random.randrange(0,10000))+"','tensach"+str(random.randrange(0,10000))+"','"+str(random.randrange(300,1000))+"','manxb"+str(random.randrange(0,100))+"');\n"
    #s3="insert into tacgia (matg,tentg,sdt,diachi) values('matg"+str(random.randrange(0,1000))+"','tentg"+str(random.randrange(0,1000))+"','"+str(random.randrange(100000000,1000000000))+"','diachi"+str(random.randrange(0,10000))+"');\n"
    s3="insert into tacgia_sach (matg,masach) values('matg"+str(random.randrange(0,1000))+"','masach"+str(random.randrange(0,10000))+"');\n"
    #s3="insert into nxb (manxb,tennxb,sdt,diachi) values('manxb"+s2+"','tennxb"+s2+"','"+str(random.randrange(1000000000,9999999999))+"','diachi"+str(random.randrange(0,10000))+"');\n"
    file.write(s3)
    pass
# Đóng file
file.close()

#sach: 10000
#tg: 1000
#nxb: 100
#tgsach: 50k