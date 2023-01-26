namespace App

open Feliz
open Feliz.Router
open System

module MyTests =
    open Fable.Core
    open Fable.Core.JsInterop
    open Fable.FormValidation
    open Browser

    type Model = {
        FName: string
        LName: string
        Email: string
        BirthDate: DateTime option
    }

    type prop with
        static member variant (value: string) = Interop.mkAttr "variant" value
        static member error (value: bool) = Interop.mkAttr "error" value

    type Components =
        /// <summary>
        /// A stateful React component that maintains a counter
        /// </summary>
        [<ReactComponent>]
        static member Counter() =
            let (count, setCount) = React.useState(0)
            Html.div [
                Html.h1 count
                Html.button [
                    prop.onClick (fun _ -> setCount(count + 1))
                    prop.text "Increment"
                ]
            ]

        [<ReactComponent(import="default", from="react-markdown")>]
        static member Markdown(children: string, remarkPlugins: obj seq) = React.imported()

        [<ImportDefault("remark-gfm")>]
        static member RemarkGfm(): obj = jsNative

        [<ReactComponent(import="FormControl", from="@mui/material")>]
        static member FormControl(error: bool) (children: ReactElement) = React.imported()

        [<ReactComponent(import="Input", from="@mui/material")>]
        static member Input(props: IReactProperty seq) = React.imported()

        [<ReactComponent(import="InputLabel", from="@mui/material")>]
        static member InputLabel(props: IReactProperty seq) (children: ReactElement) = React.imported()

        [<ReactComponent(import="FormHelperText", from="@mui/material")>]
        static member FormHelperText (props: IReactProperty seq) (children: ReactElement) = React.imported()

        [<ReactComponent(import="Button", from="@mui/material")>]
        static member Button(variant: string, ``type``: string) (children: ReactElement) = React.imported()

        [<ReactComponent>]
        static member Form() =
            let init = {
                FName = ""
                LName = ""
                Email = ""
                BirthDate = Some DateTime.Today
            }

            let hasError = false

            let model, setModel = React.useState init
            let rulesFor, validate, resetValidation, errors = useValidation()

            let handleSubmit (event: Types.Event) =
                event.preventDefault()

                console.log(errors)

                if validate() then
                    console.log("succesfully validated")

            Html.form [
                prop.onSubmit handleSubmit

                prop.children [
                    Components.FormControl(hasError)
                        (React.fragment [
                            Components.InputLabel
                                [ prop.htmlFor "first-name" ]
                                (Html.text "First Name")

                            // TODO: figure out how to pass ref to material UI
                            Components.Input [
                                prop.id "first-name"
                                prop.ref (rulesFor "First Name" [Required; MaxLen 20])
                                prop.value model.FName
                                prop.onChange (fun (e: string) -> setModel { model with FName = e })
                                prop.ariaDescribedBy "helper-text"
                            ]

                            Components.FormHelperText
                                [ prop.id "helper-text" ]
                                (Html.text "Hey! You should input your first name in here")
                        ])

                    Components.Button("contained", "submit") (Html.text "Submit")
                ]
            ]

        /// <summary>
        /// The simplest possible React component.
        /// Shows a header with the text Hello World
        /// </summary>
        [<ReactComponent>]
        static member HelloWorld() =
            let markdownMessage = "Just a link: https://reactjs.com."

            Html.div [
                // Html.h1 "Hello World"
                Components.Form()
                // Components.Markdown(children=markdownMessage, remarkPlugins=[ Components.RemarkGfm ])
                // Html.h1 "Bye World"
            ]


        /// <summary>
        /// A React component that uses Feliz.Router
        /// to determine what to show based on the current URL
        /// </summary>
        [<ReactComponent>]
        static member Router() =
            let (currentUrl, updateUrl) = React.useState(Router.currentUrl())
            React.router [
                router.onUrlChanged updateUrl
                router.children [
                    match currentUrl with
                    | [ ] -> Html.h1 "Index"
                    | [ "hello" ] -> Components.HelloWorld()
                    | [ "counter" ] -> Components.Counter()
                    | otherwise -> Html.h1 "Not found"
                ]
            ]
