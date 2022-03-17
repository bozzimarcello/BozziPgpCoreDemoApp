# Procedura di verifica della firma digitale e checksum

## Kleopatra
Download:
1. scarica il file
2. scarica la firma digitale (.sig)
3. scarica la chiave pubblica (.asc/.key)
4. importa la chiave pubblica
5. certifica la chiave pubblica (andrebbe fatto con un mezzo diverso dal web)
6. verifica il file scaricato

## Open Suse
1. scarica il file openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso
2. scarica la checksum openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256
3. scarica la firma della checksum openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256.asc
4. importa la chiave pubblica usando le ultime otto cifre della fingerprint riportata sulla pagina di download, con il comendo:
gpg --recv-keys 3DBDC284
5. verifica la chiave importata, confrontando il risultato del seguente comando con quanto riportato sul sito:
gpg --fingerprint 3DBDC284
6. verifica la checksum, il comando su Linux
sha256sum -c openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256
va sostituito dal seguente su Windows
certutil -hashfile .\openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso SHA256
confonta il risultato del comando con il codice contenuto nel file openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256
devono corrispondere
7. verifica la firma digitale con il comando
gpg --verify openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256.asc openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256
