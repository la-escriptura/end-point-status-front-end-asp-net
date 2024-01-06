Imports System.Net
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography
Imports System.Web
Public Class modMain
    Implements IHttpModule

    Public Enum PathPart As Integer
        AtBegin = -1
        AtEnd = 1
        AtBoth = 0
    End Enum

    Public Enum DBEnvironment
        Development
        Production
        HighPermission
    End Enum

    Private WithEvents _context As HttpApplication

    ''' <summary>
    '''  You will need to configure this module in the Web.config file of your
    '''  web and register it with IIS before being able to use it. For more information
    '''  see the following link: https://go.microsoft.com/?linkid=8101007
    ''' </summary>
#Region "IHttpModule Members"

    Public Sub Dispose() Implements IHttpModule.Dispose

        ' Clean-up code here

    End Sub

    Public Sub Init(ByVal context As HttpApplication) Implements IHttpModule.Init
        _context = context
    End Sub

#End Region

    Public Sub OnLogRequest(ByVal source As Object, ByVal e As EventArgs) Handles _context.LogRequest

        ' Handles the LogRequest event to provide a custom logging 
        ' implementation for it

    End Sub

    Public Shared Function ManageAddress(Address As String, Separator As String, Optional Part As PathPart = PathPart.AtEnd) As String
        If (Len(Address) = 0) Then Address = Separator
        If (Part <= 0) Then Address = IIf(Address(0) = Separator, "", Separator) & Address
        If (Part >= 0) Then Address &= IIf(Address(Len(Address) - 1) = Separator, "", Separator)
        Return Address
    End Function

    Public Shared Function ConnectionString(Optional Env As DBEnvironment = DBEnvironment.Production) As String
        Dim dataSource As String = ConfigurationManager.AppSettings("Data Source")
        Dim initialCatalog As String = ConfigurationManager.AppSettings("Initial Catalog")
        Dim integratedSecurity As String = ConfigurationManager.AppSettings("Integrated Security").Trim
        If (integratedSecurity = String.Empty) Then
            Dim userid As String = ConfigurationManager.AppSettings("User id")
            Dim password As String = Decrypt(ConfigurationManager.AppSettings("Password"))
            If (Env = DBEnvironment.Development) Then
                initialCatalog += "_Dev"
            ElseIf (Env = DBEnvironment.HighPermission) Then
                initialCatalog = "HighPermissionDB"
            End If
            Return "Data Source=" & dataSource & ";Initial Catalog=" & initialCatalog & ";User id=" & userid & ";Password=" & password & ";"
        Else
            Return "Data Source=" & dataSource & ";Initial Catalog=" & initialCatalog & ";Integrated Security=" & integratedSecurity & ";"
        End If
    End Function

    Public Shared Function Decrypt(ByVal cipherText As String) As String
        Try
            Dim passPhrase As String = ""
            Dim saltValue As String = ""
            Dim hashAlgorithm As String = "SHA1"
            Dim passwordIterations As Integer = 2
            Dim initVector As String = ""
            Dim keySize As Integer = 256
            ' Convert strings defining encryption key characteristics into byte
            ' arrays. Let us assume that strings only contain ASCII codes.
            ' If strings include Unicode characters, use Unicode, UTF7, or UTF8
            ' encoding.
            Dim initVectorBytes As Byte() = Encoding.ASCII.GetBytes(initVector)
            Dim saltValueBytes As Byte() = Encoding.ASCII.GetBytes(saltValue)
            ' Convert our ciphertext into a byte array.
            Dim cipherTextBytes As Byte() = Convert.FromBase64String(cipherText)
            ' First, we must create a password, from which the key will be
            ' derived. This password will be generated from the specified
            ' passphrase and salt value. The password will be created using
            ' the specified hash algorithm. Password creation can be done in
            ' several iterations.
            Dim password As New PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations)
            ' Use the password to generate pseudo-random bytes for the encryption
            ' key. Specify the size of the key in bytes (instead of bits).
            Dim keyBytes As Byte() = password.GetBytes(keySize \ 8)
            ' Create uninitialized Rijndael encryption object.
            Dim symmetricKey As New RijndaelManaged()
            ' It is reasonable to set encryption mode to Cipher Block Chaining
            ' (CBC). Use default options for other symmetric key parameters.
            symmetricKey.Mode = CipherMode.CBC
            ' Generate decryptor from the existing key bytes and initialization
            ' vector. Key size will be defined based on the number of the key
            ' bytes.
            Dim decryptor As ICryptoTransform = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes)
            ' Define memory stream which will be used to hold encrypted data.
            Dim memoryStream As New MemoryStream(cipherTextBytes)
            ' Define cryptographic stream (always use Read mode for encryption).
            Dim cryptoStream As New CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)
            ' Since at this point we don't know what the size of decrypted data
            ' will be, allocate the buffer long enough to hold ciphertext;
            ' plaintext is never longer than ciphertext.
            Dim plainTextBytes As Byte() = New Byte(cipherTextBytes.Length - 1) {}
            ' Start decrypting.
            Dim decryptedByteCount As Integer = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length)
            ' Close both streams.
            memoryStream.Close()
            cryptoStream.Close()
            memoryStream.Dispose()
            cryptoStream.Dispose()
            ' Convert decrypted data into a string.
            ' Let us assume that the original plaintext string was UTF8-encoded.
            Dim plainText As String = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount)
            ' Return decrypted string.
            Return plainText
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function NothingtoDBNull(value As String) As Object
        If (value.Trim = "") Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function

    Public Shared Function DBNulltoNothing(value As Object) As Object
        If (IsDBNull(value)) Then
            Return Nothing
        Else
            Return value
        End If
    End Function
End Class
