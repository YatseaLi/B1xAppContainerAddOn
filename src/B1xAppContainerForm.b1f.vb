Option Strict Off
Option Explicit On

Imports SAPbouiCOM.Framework

Namespace B1xAppContainerAddOn
    <FormAttribute("B1xAppContainerAddOn.B1xAppContainerForm", "B1xAppContainerForm.b1f")>
    Friend Class B1xAppContainerForm
        Inherits UserFormBase
        Private WithEvents oItem As SAPbouiCOM.Item
        Private targetUrl As String

        Private Sub New()
        End Sub

        'Generate a UI Form with an WebBrowser Item based on the given WebAppInfo
        Public Sub New(ByVal webAppInfo As WebAppInfo)
            Me.UIAPIRawForm.Title = webAppInfo.formTitle

            Me.targetUrl = webAppInfo.targetUrl
            'Add a WebBrowser item
            oItem = Me.UIAPIRawForm.Items.Add("WebBrowser", SAPbouiCOM.BoFormItemTypes.it_WEB_BROWSER)
            oItem.Left = 10
            oItem.Top = 10
            oItem.Width = (Me.UIAPIRawForm.Width - 20)
            oItem.Height = (Me.UIAPIRawForm.Height - 30)
            Dim oWebBrowser As SAPbouiCOM.WebBrowser = CType(oItem.Specific, SAPbouiCOM.WebBrowser)
            oWebBrowser.Url = webAppInfo.targetUrl
        End Sub

        Public Overrides Sub OnInitializeComponent()
            
            Me.OnCustomInitialize()

        End Sub

        Public Overrides Sub OnInitializeFormEvents()
            AddHandler ResizeAfter, AddressOf Me.Form_ResizeAfter

        End Sub

        Private Sub OnCustomInitialize()

        End Sub
        Private Sub Form_ResizeAfter(ByVal pVal As SAPbouiCOM.SBOItemEventArg)

            oItem.Width = (Me.UIAPIRawForm.Width - 10)
            oItem.Height = (Me.UIAPIRawForm.Height - 20)

        End Sub
    End Class
End Namespace
