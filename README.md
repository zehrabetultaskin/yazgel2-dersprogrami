Kullanılan Teknolojiler:
1) C# (C Sharp):
C#, Microsoft tarafından geliştirilen bir programlama dilidir ve genellikle Windows tabanlı uygulamaların geliştirilmesinde kullanılır. C#, nesne odaklı bir dil olup güçlü bir şekilde tip güvenliği ve modern programlama özelliklerini destekler. Programın temel yazılım altyapısı C# dilinde geliştirilmiştir, bu da programın güvenilir, hızlı ve ölçeklenebilir olmasını sağlar.
2) .NET Framework:
.NET Framework, Microsoft'un geliştirdiği bir yazılım platformudur ve C# gibi dillerle yazılmış uygulamaların çalıştığı ortamı sağlar. Bu platform, çeşitli kütüphaneleri, API'ları ve araçları içerir. .NET Framework, programın geliştirilmesi ve çalıştırılması için temel altyapıyı oluşturur. Ayrıca, .NET'in sunduğu zengin sınıf kütüphaneleri, kodun daha etkili ve hızlı bir şekilde yazılmasına olanak tanır.
3) Visual Studio:
Visual Studio, Microsoft'un entegre geliştirme ortamıdır. C# gibi dillerle yazılan projelerin tasarımından derlenmesine ve hata ayıklanmasına kadar birçok geliştirme sürecini destekler. Geliştiricilere zengin bir kod editörü, hata ayıklayıcı, derleyici ve arayüz tasarım araçları sağlar. Ayrıca, Visual Studio'nun zengin eklenti ekosistemi ve entegre takım işbirliği özellikleri, yazılım geliştirme sürecini daha verimli hale getirir.
4) Microsoft SQL Server (MSSQL):
Microsoft SQL Server, ilişkisel veritabanı yönetim sistemidir ve programın veritabanı yönetimini sağlamak için kullanılır. MSSQL, güvenilirlik, performans ve ölçeklenebilirlik açısından zengin bir özellik setine sahiptir. Bu veritabanı sistemi, programın öğretmen, derslik, sınıf ve ders bilgilerini depolamak, sorgulamak ve yönetmek için kullanılır. Veritabanı işlemleri, programın genel performansını ve veri bütünlüğünü sağlamak için MSSQL üzerinden gerçekleştirilir.

1-) Giriş Ekranı : 

Projemizi ilk çalıştırdığımızda karşımıza gelen araryüzümüzde hangi veritabanı sunucusu ve veritabanı ile çalışacaksak onun bağlantı adresini veriyoruz ya da hazır bir veritabanımız yok ise sunucu adresini yazarak yeni bir veritabanı oluşturarak devam ediyoruz.

![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/07812533-4e86-4bf6-94f2-7867fde459db)

2-) Ders ve Öğretmen Ekranları : 

Projemizde gerekli veritabanı bağlantılarını uyguladıktan sonra sistemimizde yer alacak olan derslerimizi,öğretmenlerimizi ,dersliklerimizi ve sınıflarımızı ekleyerek gerekli dersler hangi sınıflarda hangi öğretmenler ve dersliklerde olacak şekilde ders atamamızı yapabiliyor olacağız.Sistemizde yer alacak olan dersler ve öğretmenler konusunda çeşitli kısıtlamalara gidilebilir aynı zamanda hangi gün uygun olacaklarını ya da olmayacaklarını sistem üzerinden belirleyebiliyor olacağız.

![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/50f9d117-ad1c-406c-9c2d-bddb77c07295)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/248bf838-dbca-43d8-901e-31fe2c29cea7)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/7f88e2a3-321d-4249-b63a-7609238e5f9b)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/ec7f3a41-d0c1-4553-a348-85a7dc56fdc9)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/a3912bc1-e724-431d-88a5-eb67e549c853)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/98f1b0e4-8f51-42b2-a8bf-4ee8ffb38eef)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/29e4eff3-6f24-4b27-b4c0-fd282d4a29c6)

3-) Ders Programı Oluşturma Aracı : 

Projemizde gerekli haftalık ders dağılım şeklini öğretmenlerin müsaitlik durumunu ve nerede ders vereceklerini sistemimizde belirledikten sonra ders programı oluşturma aracımız ile gerekli konfigurasyonalara sahip haftalık ders çizelgemiz oluşturulmaktadır.Dersleri çizelge üzerinden günlerini veya saatlarini değiştirebilme özelliğimiz mevcuttur ama çakışan derslerimiz aynı zaman aralığında çizelge üzerinde yer alamazlar.

![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/857a264d-b4e0-4331-b273-476c9a20c999)

Proje Kurulumu : 

Projemizi çalıştıracağımız makineye indirdikten sonra visual studio ile projemizi çalıştırıyoruz. Projemiz çalıştıktan sonra karşımıza veritabanı bağlantı ekranı aracı ile bizi karşılıyor. Gerekli sunucu ve veritabanı bağlantılarını veritabanı kurulum aracını kullanarak yaptıktan sonra ekranlarımız arasında ilerlemeye devam ederek,ders,öğretmen,sınıf ve derslik eklemelerini yaparak ve bunlar arasında gerekli ders atamalarını ve kısıtları uyguladıktan sonra ders programızı projemiz oluşturmaktadır.

![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/a21bb487-ebb3-45ab-8532-e3dc1add54b1)
![image](https://github.com/zehrabetultaskin/yazgel2-dersprogrami/assets/74192618/9c3cf46a-f43f-451c-906d-e7ac4a7db333)

