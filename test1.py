import redis
import random
import time

r = redis.Redis(host="localhost",port=6379,db=0)

listTenSach = ["In Search of Lost Time by Marcel Proust","Don Quixote by Miguel de Cervantes","Ulysses by James Joyce","The Great Gatsby by F. Scott Fitzgerald",
            "Moby Dick by Herman Melville","Hamlet by William Shakespeare","War and Peace by Leo Tolstoy","The Odyssey by Homer","One Hundred Years of Solitude by Gabriel Garcia Marquez",
            "The Divine Comedy by Dante Alighieri","The Brothers Karamazov by Fyodor Dostoyevsky","Madame Bovary by Gustave Flaubert","The Adventures of Huckleberry Finn by Mark Twain",
            "The Iliad by Homer","Lolita by Vladimir Nabokov"]
listTen = ["Livinus","James","Michael","David","John","Kevin","Robert","Thomas","Emily","Mark","Matthew","Anthony","Anna",
            "Daniel","William","Jessica","Brian","Elizabeth","Christopher","Paul","Jennifer","Stephen","Emma","Alexander",
            "Sarah","Joseph","Chaima","Dennis","Maria","Rebecca","Ashley","Ryan","Patrick","Jeffrey","Charles","Richard","Andrea",
            "Heather","Michelle","Taylor","Rachel","Laura","Kimberly","Linda","Andrew"]
listEmail = ["@gmail.com","@outlook.com","@yahoomail.com","@yanindex.com","@ProtonMail.com","@Gmx.com","@Mail.com"]

def insertSACH():
    for i in range(0,10000):
        tenSach = str(listTenSach[random.randint(0,len(listTenSach)-1)])
        soTrang = str(random.randint(1,1000))
        MSNXB = "NXB" + str(random.randint(1,100))
        key = "sach" + str(i +1)
        values = tenSach +"," + soTrang + ","+ MSNXB
        r.set(key,values)
        print(key + ',' + values)

def insertTACGIA():
    for i in range(0,10000):
        tenTacGia = str(listTen[random.randint(0,len(listTen)-1)])
        soDT = "0987" + str(random.randint(10000,100000))
        eMail = tenTacGia + str(listEmail[random.randint(0,len(listEmail)-1)])
        key = "tacgia" + str(i +1)
        values = tenTacGia + "," + soDT +"," + eMail
        r.set(key,values)
        print(key + ',' +  values)

def insertTACGIASACH():
    for i in range(0,50000):
        MSTG = "tacgia" + str(i+1)
        MSSACH = "sach" + str(i+1)
        key = "tgs" + str(i+1)
        values = MSTG + "," + MSSACH
        r.set(key,values)
        print(key + ',' + values)

def insertNXB():
    for i in range(0,100):
        tenNXB = "TENNXB" + str(i+1)
        eMail = tenNXB + str(listEmail[random.randint(0,len(listEmail)-1)])
        soDT = "0987" + str(random.randint(10000,100000))
        key = "NXB" + str(i + 1)
        values = tenNXB + "," + soDT + "," + eMail
        r.set(key,values)
        print(key + ',' + values)

def select_not_where():
    start_time1 = time.time()
    print("select khong co dieu kien")    
    # tim tat ca cac nha xuat ban
    for key in r.scan_iter("NXB*"):
        values = r.get(key)
        # format thanh kieu tring
        values = key.decode("UTF8") + "," + values.decode("UTF8")
        print(values)
    end_time1 = time.time()
    print ('total time insert data: %f ms' % ((end_time1 - start_time1) * 1000))
     

def select_in_pk():
    start_time2 = time.time()
    print("select co dieu kien tren khoa chinh")    
    # tim thong tin cua quyen sach co ms "sach69"
    values = r.get("sach69")
    values = "sach69" + "," + values.decode("UTF8")
    print(values)
    end_time2 = time.time()
    print ('total time insert data: %f ms' % ((end_time2 - start_time2) * 1000))

def select_notin_pk():
    start_time3 = time.time()
    print("select co dieu kien tren khong khoa chinh")    
    # tim tac gia co email la Heather@yahoomail.com
    values = ""
    for key in r.scan_iter("*"):
        temp = r.get(key).decode("UTF8")
        if ("Heather@yahoomail.com" in temp):
            values = key.decode("UTF8") + "," + temp
            print(values)
    if(values == ""):
        print("ko tim thay tac gia co email Heather@yahoomail.com")
    end_time3 = time.time()
    print ('total time insert data: %f ms' % ((end_time3 - start_time3) * 1000))
        
def Update_not_where():
    start_time4 = time.time()
    print("update khong where")   
    print("update not where")
    end_time4 = time.time()
    print ('total time insert data: %f ms' % ((end_time4 - start_time4) * 1000))
def Update_notin_pk():
    start_time5 = time.time()
    print("upadate co where tren khong khoa chinh")   
    # doi ten nhung quyen sach co ten la: 'Don Quixote by Miguel de Cervantes'
    sach_cu = 'Moby Dick by Herman Melville'
    sach_moi = 'sach da duoc doi ten'
    for key in r.scan_iter('*'):
       temp = r.get(key).decode('utf-8')
       if (sach_cu in temp):
           values = temp.replace(sach_cu, sach_moi)
           r.set(key,values)
    end_time5 = time.time()
    print ('total time insert data: %f ms' % ((end_time5 - start_time5) * 1000))

def Update_in_pk():
    start_time6 = time.time()
    print("upadate co where tren khoa chinh")   
    # cap nhat sodienthoai thanh 123456789  khi mstg = tacgia10
    key = 'tacgia10'
    temp = r.get(key).decode('utf-8').split(',')
    sdt = temp[1]
    if(sdt.isdigit() ==True):
        values = r.get(key).decode('utf-8').replace(sdt,'123456789')
        r.set(key,values)
    end_time6 = time.time()
    print ('total time insert data: %f ms' % ((end_time6 - start_time6) * 1000))

def Delete_in_pk():
    start_time7 = time.time()
    print("delete tren khoa chinh")   
    # delete quyen sach co ma so la 'sach100'
    for key in r.scan_iter('sach*'):
        if(key.decode('utf-8') == 'sach1001'):
            r.delete(key)
    end_time7 = time.time()
    print ('total time insert data: %f ms' % ((end_time7 - start_time7) * 1000))

def Delete_notin_pk():
    start_time8 = time.time()
    print("delete tren khong khoa chinh")
    # delete nhung cuon sach co ten la: 'In Search of Lost Time by Marcel Proust'
    for key in r.scan_iter('sach*'):
        values = r.get(key).decode('utf-8')
        if ('In Search of Lost Time by Marcel Proust' in values):
            r.delete(key)
    end_time8 = time.time()
    print ('total time insert data: %f ms' % ((end_time8 - start_time8) * 1000))

def Delete_not_where():
    start_time9 = time.time()
    print("delete tren khong where")
    # delete toan bo bang tac gia sach
    for key in r.scan_iter('tgs*'):
        r.delete(key)
    end_time9 = time.time()
    print ('total time insert data: %f ms' % ((end_time9 - start_time9) * 1000))

def Insert():
    start = time.time()
    insertSACH()
    insertTACGIA()
    insertTACGIASACH()
    insertNXB()
    finish = time.time()
    print ('Tong thoi gian insert data: %f ms' % ((finish - start) * 1000))
#def select():
    print("select khong co dieu kien")
    select_not_where()
    print("select co dieu kien tren khoa chinh")
    select_in_pk()
    print("select co dieu kien khong tren khoa chinh")
    select_notin_pk()
#def Update():
    # Update_not_where()
    Update_in_pk()
    Update_notin_pk()
#def Delete():
    begin = time.time()
    Delete_not_where()
    # Delete_in_pk()
    # Delete_notin_pk()
    end = time.time()
    print ('Thoi gian delete mot doi tuong co dieu kien nam tren khoa chinh la: %f ms' % ((end - begin) * 1000))


def main():
    # start_time = time.time()
    Insert()
    select_not_where()
    select_in_pk()
    select_notin_pk()
    Update_not_where()
    Update_notin_pk()
    Update_in_pk()
    Delete_in_pk()
    Delete_notin_pk()
    Delete_not_where()

    
    # Select()
    # Update()
    # Delete()
    # end_time = time.time()
    # print ('total time insert data: %f ms' % ((end_time - start_time) * 1000))
if __name__ == '__main__':
    main()
    print("finish program")
