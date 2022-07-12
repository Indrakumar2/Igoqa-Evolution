import-module webadministration
#$mypwd = ConvertTo-SecureString -String "7xT7MCurah5TVNMZ" -Force –AsPlainText
$mypwd = convertto-securestring "7xT7MCurah5TVNMZ" -asplaintext -force
cd cert:
#$cert = New-SelfSignedCertificate -DnsName myweb -Friendlyname MyCert -CertStoreLocation Cert:\LocalMachine\My
#$mypwd = ConvertTo-SecureString -String "7xT7MCurah5TVNMZ" -Force –AsPlainText
$cert = Import-PfxCertificate -FilePath C:\inetpub\wwwroot\SSL.pfx -CertStoreLocation Cert:\LocalMachine\My -Password $mypwd
$rootStore = New-Object System.Security.Cryptography.X509Certificates.X509Store -ArgumentList Root, LocalMachine

$rootStore.Open("MaxAllowed")
$rootStore.Add($cert)
$rootStore.Close()

cd iis:
new-item -path IIS:\SslBindings\0.0.0.0!443 -value $cert
New-WebBinding -Name "Default Web Site" -IP "*" -Port 443 -Protocol https
iisreset


