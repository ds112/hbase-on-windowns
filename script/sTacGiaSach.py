# Mở file
file = open("putTacGiaSach.txt", "w")
for num in range(1,50001):
    s2=str(num)
    s1="put 'TACGIASACH','TGS" + s2 + "','TGS:MSTG','MSTG" + s2 + "'\n"
    s3="put 'TACGIASACH','TGS" + s2 + "','TGS:MSS','MSS" + s2 + "'\n"
    file.write(s1)
    file.write(s3)
    pass
# Đóng file
file.close()