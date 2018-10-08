# Mở file
file = open("putNXB.txt", "w")
for num in range(1,50001):
    s2=str(num)
    s3="put 'NXB','NXB" + s2 + "','NXB:MSTG','MS-NXB" + s2 + "'\n"
    s4="put 'NXB','NXB" + s2 + "','NXB:TENTG','TEN-NXB" + s2 + "'\n"
    s5="put 'NXB','NXB" + s2 + "','NXB:SDT','SDT-NXB" + s2 + "'\n"
    s6="put 'NXB','NXB" + s2 + "','NXB:EMAIL','EMAIL-NXB" + s2 + "@DS112.COM'\n"
    file.write(s3)
    file.write(s4)
    file.write(s5)
    file.write(s6)
    pass
# Đóng file
file.close()