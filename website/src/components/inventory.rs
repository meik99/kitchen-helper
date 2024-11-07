use dioxus::prelude::*;


#[component]
pub fn Inventory() -> Element {
    rsx! {
        section { class: "pf-v6-c-page__main-section",
            div { class: "pf-v6-c-page__main-body",                
                h1 { 
                    class: "pf-v6-c-title pf-m-h1",
                    "Inventory" 
                }
            }
        }
    }
}
