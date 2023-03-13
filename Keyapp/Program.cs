using Azure.Identity;
using Azure.Security.KeyVault.Keys;
using Azure.Security.KeyVault.Keys.Cryptography;
using System.Text;

string tenantId = "";
string clientId = "";
string clientSecret = "";

ClientSecretCredential clientSecretCredential = new(tenantId, clientId, clientSecret);

string keyVaultUrl = "https://gsam-az204-keyvault.vault.azure.net/";
string keyName = "appkey";
string textToEncrypt = "This is a secret text";

KeyClient keyClient = new(new Uri(keyVaultUrl), clientSecretCredential);

var key = keyClient.GetKey(keyName);

CryptographyClient cryptographyClient = new(key.Value.Id, clientSecretCredential);

byte[] textToBytes = Encoding.UTF8.GetBytes(textToEncrypt);

EncryptResult result = cryptographyClient.Encrypt(EncryptionAlgorithm.RsaOaep, textToBytes);

Console.WriteLine("The encrypted string");
Console.WriteLine(Convert.ToBase64String(result.Ciphertext));

byte[] cipherToBytes = result.Ciphertext;

DecryptResult textDecrypted = cryptographyClient.Decrypt(EncryptionAlgorithm.RsaOaep, cipherToBytes);

Console.WriteLine(Encoding.UTF8.GetString(textDecrypted.Plaintext));

Console.ReadKey();