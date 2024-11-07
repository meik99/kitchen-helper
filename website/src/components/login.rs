#[path = "../api/mod.rs"]

use dioxus::prelude::*;
use dioxus_logger::tracing::{info, Level, error};

use crate::api;

#[component]
pub fn Login(mut use_token: Signal<api::login::Token>) -> Element {    
    let mut email = use_signal(|| "".to_string());
    let mut password = use_signal(|| "".to_string());    

    let log_in = move |_| {
        spawn(async move {
            let entered_email: String = email.read().clone();
            let entered_password: String = password.read().clone();
            let result = api::login::get_token(entered_email.as_str(), entered_password.as_str()).await;

            match result {
                Ok(token) => {
                    info!("{}", token.token);
                    *use_token.write() = token;
                },
                Err(err) => error!("{}", err),                
            }
        });
    };

    rsx!(
        div {
            style: "--pf-v6-c-background-image--BackgroundImage: url(/assets/images/pf-background.svg)",
            class: "pf-v6-c-background-image"
        }
        div { class: "pf-v6-c-login",
            div { class: "pf-v6-c-login__container",                
                main { class: "pf-v6-c-login__main",
                    header { class: "pf-v6-c-login__main-header",
                        h1 { class: "pf-v6-c-title pf-m-3xl", "Log in to Knive" }
                    }
                    div { class: "pf-v6-c-login__main-body",
                        form { novalidate: "false", class: "pf-v6-c-form",
                            div {
                                "aria-live": "polite",
                                class: "pf-v6-c-form__helper-text",
                                div { class: "pf-v6-c-helper-text pf-m-hidden",
                                    div {
                                        class: "pf-v6-c-helper-text__item pf-m-error",
                                        id: "-helper",
                                        span { class: "pf-v6-c-helper-text__item-icon",
                                            i {
                                                "aria-hidden": "true",
                                                class: "fas fa-fw fa-exclamation-circle"
                                            }
                                        }
                                        span { class: "pf-v6-c-helper-text__item-text",
                                            "Invalid login credentials."
                                        }
                                    }
                                }
                            }
                            div { class: "pf-v6-c-form__group",
                                label {
                                    r#for: "login-demo-form-username",
                                    class: "pf-v6-c-form__label",
                                    span { class: "pf-v6-c-form__label-text", "Username" }
                                    span {
                                        "aria-hidden": "true",
                                        class: "pf-v6-c-form__label-required",
                                        "*"
                                    }
                                }
                                span { class: "pf-v6-c-form-control pf-m-required",
                                    input {
                                        required: "false",
                                        r#type: "text",
                                        "input": "true",
                                        name: "email",
                                        id: "email",
                                        value: "{email}",
                                        oninput: move |event| email.set(event.value())
                                    }
                                }
                            }
                            div { class: "pf-v6-c-form__group",
                                label {
                                    r#for: "login-demo-form-password",
                                    class: "pf-v6-c-form__label",
                                    span { class: "pf-v6-c-form__label-text", "Password" }
                                    span {
                                        "aria-hidden": "true",
                                        class: "pf-v6-c-form__label-required",
                                        "*"
                                    }
                                }
                                span { class: "pf-v6-c-form-control pf-m-required",
                                    input {
                                        r#type: "password",
                                        name: "password",
                                        required: "false",
                                        id: "password",
                                        value: "{password}",
                                        oninput: move |event| password.set(event.value())
                                    }
                                }
                            }
                            div { class: "pf-v6-c-form__group pf-m-action",
                                button {
                                    r#type: "button",
                                    class: "pf-v6-c-button pf-m-block pf-m-primary",
                                    onclick: log_in,
                                    span { class: "pf-v6-c-button__text", "Log in" }
                                }
                            }
                        }
                    }
                }
            }
        }
    )
}
