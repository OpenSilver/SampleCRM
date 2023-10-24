Imports System.ComponentModel.DataAnnotations

Namespace Models
    Public Class CountModel
        <Key>
        Public Property Id As Integer
        Public Property CategoryCount As Integer
        Public Property CustomerCount As Integer
        Public Property OrderCount As Integer
        Public Property ProductCount As Integer
    End Class
End Namespace