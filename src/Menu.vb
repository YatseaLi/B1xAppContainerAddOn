Imports SAPbouiCOM.Framework
Imports System.Text

Namespace B1xAppContainerAddOn

    Structure WebAppInfo
        Public targetUrl As String
        Public formTitle As String
    End Structure

    Public Class Menu

        Private WithEvents SBO_Application As SAPbouiCOM.Application = Nothing

        Sub New()
            SBO_Application = Application.SBO_Application
        End Sub

        Sub AddMenuItems()
            'Remove the menu if already exist.
            RemoveMenus()

            Dim menuXml As Xml.XmlDocument = New Xml.XmlDocument
            menuXml.Load("B1xAppMenus.xml")
            Application.SBO_Application.LoadBatchActions(String.Format(menuXml.InnerXml, Windows.Forms.Application.StartupPath))

        End Sub

        Function GetTargetWebAppInfo(ByVal menuUid As String)
            Dim menuXml As Xml.XmlDocument = New Xml.XmlDocument
            menuXml.Load("B1xAppMenus.xml")

            Dim menuNode As Xml.XmlNode = menuXml.SelectSingleNode(String.Format("//Menu[@UniqueID='{0}' and @Type='1']", menuUid))

            Dim sb As StringBuilder = New StringBuilder
            Dim targetUrl As String = menuNode.SelectSingleNode("./@TargetWebAppFullUrl").InnerText
            'menuXml.SelectSingleNode(String.Format("//Menu[@UniqueID='{0}' and @Type='1']/@TargetWebAppFullUrl", menuUid)).InnerText

            If String.IsNullOrEmpty(targetUrl) Then
                sb.Append(Application.SBO_Application.XSEngineBaseURL)
                targetUrl = menuNode.SelectSingleNode("./@TargetHANAxApp").InnerText
                sb.Append(targetUrl)
            Else
                sb.Append(targetUrl)
            End If

            If String.IsNullOrEmpty(sb.Length = 0) Then
                Throw New Exception("Invalid target url")
            End If

            Dim passB1Cred As String = menuNode.SelectSingleNode("./@PassB1CredInUrl").InnerText
            If passB1Cred.Equals("true") Then
                sb.Append(String.Format("?CompanyDB={0}&User={1}&Language={2}", Application.SBO_Application.Company.DatabaseName, _
                                        Application.SBO_Application.Company.UserName, Application.SBO_Application.Language))
            End If

            'Todo: Please uncomment below if you would like to have Service Layer SSO from UI API.
            'Refer to this blog post for details https://blogs.sap.com/2016/07/01/sap-business-one-service-layer-sso-with-ui-api/
            ' Dim passSLContextInUrl As String = menuNode.SelectSingleNode("./@PassSLContextInUrl").InnerText
            ' If passSLContextInUrl.Equals("true") Then
            '     Dim serviceLayerAddress As String = "https://<YOUR_HANA_SERVER>:50000/b1s/v1"
            '     Dim sConnectionContext As String = SBO_Application.Company.GetServiceLayerConnectionContext(serviceLayerAddress)

            '     If(passB1Cred) Then
            '         sb.Append(String.Format("&{0}", sConnectionContext))
            '     Else
            '         sb.Append(String.Format("?{0}", sConnectionContext))
            '     End If    
            ' End If

            Dim webAppInfo As WebAppInfo = New WebAppInfo
            webAppInfo.targetUrl = sb.ToString

            Dim formTitle As String = menuNode.SelectSingleNode("./@FormTitle").InnerText
            webAppInfo.formTitle = formTitle
            Return webAppInfo

        End Function

        Sub SBO_Application_MenuEvent(ByRef pVal As SAPbouiCOM.MenuEvent, ByRef BubbleEvent As Boolean) Handles SBO_Application.MenuEvent
            BubbleEvent = True

            Try
                If (pVal.BeforeAction And pVal.MenuUID.StartsWith("B1xApp_")) Then
                    Dim activeForm As B1xAppContainerForm
                    'Generate a B1xAppContainerForm and pass on the WebAppInfo
                    activeForm = New B1xAppContainerForm(GetTargetWebAppInfo(pVal.MenuUID))
                    activeForm.Show()
                    BubbleEvent = False
                End If
            Catch ex As System.Exception
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "")
            End Try

        End Sub

    End Class
End Namespace