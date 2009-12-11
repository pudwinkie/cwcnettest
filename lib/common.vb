' 驗證統一編號是否正確
'
'
'
Public Shared Function ValidateTaxID(ByVal vstrTaxID As String) As Boolean
  Dim i As Integer

  Dim a1 As Integer
  Dim a2 As Integer
  Dim a3 As Integer
  Dim a4 As Integer
  Dim a5 As Integer

  Dim b1 As Integer
  Dim b2 As Integer
  Dim b3 As Integer
  Dim b4 As Integer
  Dim b5 As Integer

  Dim c1 As Integer
  Dim c2 As Integer
  Dim c3 As Integer
  Dim c4 As Integer

  Dim d1 As Integer
  Dim d2 As Integer
  Dim d3 As Integer
  Dim d4 As Integer
  Dim d5 As Integer
  Dim d6 As Integer
  Dim d7 As Integer
  Dim cd8 As Integer


  '判斷長度
  If Len(vstrTaxID) <> 8 Then
      Return False
  End If

  '判斷字元
  For i = 1 To 8
    If InStr("0123456789", Mid(vstrTaxID, i, 1)) = 0 Then
      Return False
    End If
  Next

  '設定變數
  d1 = CInt(Mid(vstrTaxID, 1, 1))
  d2 = CInt(Mid(vstrTaxID, 2, 1))
  d3 = CInt(Mid(vstrTaxID, 3, 1))
  d4 = CInt(Mid(vstrTaxID, 4, 1))
  d5 = CInt(Mid(vstrTaxID, 5, 1))
  d6 = CInt(Mid(vstrTaxID, 6, 1))
  d7 = CInt(Mid(vstrTaxID, 7, 1))
  cd8 = CInt(Mid(vstrTaxID, 8, 1))

  c1 = d1
  c2 = d3
  c3 = d5
  c4 = cd8

  a1 = Int((d2 * 2) / 10)
  b1 = (d2 * 2) Mod 10

  a2 = Int((d4 * 2) / 10)
  b2 = (d4 * 2) Mod 10

  a3 = Int((d6 * 2) / 10)
  b3 = (d6 * 2) Mod 10

  a4 = Int((d7 * 4) / 10)
  b4 = (d7 * 4) Mod 10

  a5 = Int((a4 + b4) / 10)
  b5 = (a4 + b4) Mod 10

  '計算公式
  If (a1 + b1 + c1 + a2 + b2 + c2 + a3 + b3 + c3 + a4 + b4 + c4) Mod 10 = 0 Then
        Return True
  End If

  If d7 = 7 Then
        If (a1 + b1 + c1 + a2 + b2 + c2 + a3 + b3 + c3 + a5 + c4) Mod 10 = 0 Then
            Return True
        End If
  End If

  Return False
End Function