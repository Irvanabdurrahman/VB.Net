
Imports System.Data.SqlClient

Public Class Connect
    Public connection As String = "Data Source=localhost;Initial Catalog=TEST;User ID=sa;Password=R0m4n5yP@c;"
    Public cnn As New SqlConnection(connection)
End Class
