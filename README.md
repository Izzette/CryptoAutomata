
Name:
CryptoAutomata v1.1.0

Developer:
Copyright (c) 2015 Isabell Cowan
<isabellcowan@gmail.com>

License:
GNU -- GPL v3.0
Must be distributed with sources and a copy of the license document
If License.txt is not present, see <http://www.gnu.org/licenses/>
If src/, src.zip, or src.tar.gz are not present, see <https://github.com/Izzette/CryptoAutomata>

Description:
Simple XOR-cipher encryption for password authenticated key exchange using a cellular automata based key deriviation function.

Usage:
Run CryptoAutomata.exe
Encrypted files carry the *.crypt extention, which is removed when decrypted.
You will need to specify the folowing when asked:
	The absolute or relative filename of the file you want to encrypt or decrypt, including the extention (easier if you move file to the folder containing CryptoAutomata.exe).
	The password (the same to encrypt and decrypt the file).

Location:
Remote git repository located at <https://github.com/Izzette/CryptoAutomata.git>.
Repository formated for Monodevelop.

Enviroment:
Developed under Linux with Mono version 3.12.* for .NET v4.0.
Tested minimally on Windows 7.

Additional Dependancies (may or may not be included in distributable):
RandomAutomata v1.0 from <https://github.com/Izzette/RandomAutomata.git>.
