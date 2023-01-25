namespace App

open Feliz
open Feliz.Router

module MyTests = 
    open Fable.Core
    open Fable.Core.JsInterop
    open Browser
    
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
        static member Markdown(children: string, remarkPlugins: obj) = React.imported()
        
        [<ImportDefault("remark-gfm")>]
        static member RemarkGfm(children: string): obj = jsNative
        
        /// <summary>
        /// The simplest possible React component.
        /// Shows a header with the text Hello World
        /// </summary>
        [<ReactComponent>]
        static member HelloWorld() =
            let markdownMessage = "Just a link: https://reactjs.com."
            let plugins = [| Components.RemarkGfm |]
            console.log("click!", plugins)
            
            Html.div [
                Html.h1 "Hello World"
                Components.Markdown(children=markdownMessage, remarkPlugins=plugins)
                Html.h1 "Bye World"
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