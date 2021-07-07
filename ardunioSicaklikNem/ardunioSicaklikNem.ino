//ÖLÇÜLEN SICAKLIK VE NEM BİLGİSİNİN KABLOSUZ İLETİMİ
/* Software Serial Port Kütüphanesini Dahil Etme */
#include <SoftwareSerial.h>
/* Bluetooth modülünün TXD pini ile iletişim kurmak için */
#define BT_SERIAL_TX 10
/* Bluetooth modülünün RXD pini ile iletişim kurmak için */
#define BT_SERIAL_RX 11
/* Seri Bağlantı Noktasını Ayarlama */
SoftwareSerial BluetoothSerial(BT_SERIAL_TX, BT_SERIAL_RX);
// DHT-11 Konfigürasyonu
#include <TinyDHT.h> // TinyDHT Sensör Kütüphanesi
// Kullandığımız Sensör Tipini Tanımlama
#define DHTTYPE DHT11 // DHT 11
#define TEMPTYPE 0 // Celsius için 0,Fahrenheit için 1 Kullanılır.
#define DHTPIN 12 
DHT dht(DHTPIN, DHTTYPE); // Sıcaklık/Nem Sensörünü Tanımlama
void setup() {
/* Yazılım Seri Bağlantı Noktası İçin Baud Hızını Ayarlama */
BluetoothSerial.begin(57600); // Bluetooth'u Başlatma
delay(1000);
dht.begin(); // DHT Sıcaklık/Nem Sensörü Başlatma
BluetoothSerial.print("Başlatılıyor ...\n");
}
void loop() {
// Take readings
int8_t h = dht.readHumidity(); // Nem Değerini Okuma
int16_t t = dht.readTemperature(TEMPTYPE); // Sıcaklık Değerini Okuma
if ( t == BAD_TEMP || h == BAD_HUM ) { // Hata Koşulları Varsa (Detaylar İçin; 'TinyDHT'.)} 
    else {
        BluetoothSerial.print("Sicaklik: ");
        BluetoothSerial.print(t);
        BluetoothSerial.print(", Nem: %");
        BluetoothSerial.print(h);
        BluetoothSerial.print("\n");
    }}
