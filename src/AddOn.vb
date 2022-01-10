Imports SAPbouiCOM.Framework

Namespace B1xAppContainerAddOn
    Module AddOn

        <STAThread()>
        Sub Main(ByVal args() As String)

            Try

                Dim oApp As Application
                If (args.Length < 1) Then
                    oApp = New Application
                Else
                    oApp = New Application(args(0))
                End If

                Dim MyMenu As Menu
                MyMenu = New Menu()
                MyMenu.AddMenuItems()
                AddHandler Application.SBO_Application.AppEvent, AddressOf SBO_Application_AppEvent
                oApp.Run()

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

        End Sub

        Sub RemoveMenus()
            Dim oMenus As SAPbouiCOM.Menus = Application.SBO_Application.Menus

            If oMenus.Exists("B1xApp") Then
                oMenus.RemoveEx("B1xApp")
            End If
        End Sub

        Sub OnShutDown()
            RemoveMenus()
            System.Windows.Forms.Application.Exit()
        End Sub

        Sub SBO_Application_AppEvent(ByVal EventType As SAPbouiCOM.BoAppEventTypes)
            Select Case EventType
                Case SAPbouiCOM.BoAppEventTypes.aet_ShutDown, SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition
                    OnShutDown()
                Case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged
                Case SAPbouiCOM.BoAppEventTypes.aet_FontChanged
                Case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged

            End Select
        End Sub

    End Module
End Namespace