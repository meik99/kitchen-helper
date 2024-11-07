#[path = "../env.rs"]
mod env;

use std::collections::HashMap;
use serde::{Deserialize, Serialize};
use dioxus_logger::tracing::{info, Level};

use crate::api;

const LOGIN_PATH: &'static str = "/api/users/login";

#[derive(Clone, Debug, PartialEq, Serialize, Deserialize)]
pub struct Token {
    #[serde(flatten)]
    pub user: User,
    #[serde(default)]
    pub token: String,
    #[serde(default)]
    pub exp: i64
}

#[derive(Clone, Debug, PartialEq, Serialize, Deserialize)]
pub struct User {
    #[serde(default)]
    pub email: String,
    #[serde(default)]
    pub createdAt: String,    
    #[serde(default)]
    pub updatedAt: String,
    #[serde(default)]
    pub id: String,    
}

pub fn newToken() -> api::login::Token {
    return api::login::Token { 
            user: api::login::User {
            email: "".to_string(),
            createdAt: "".to_string(),
            updatedAt: "".to_string(),
            id: "".to_string()
        }, 
        token: "".to_string(), 
        exp: 0
    }
}

pub fn newTokenWithExpiration(exp: i64) -> api::login::Token {
    return api::login::Token { 
            user: api::login::User {
            email: "".to_string(),
            createdAt: "".to_string(),
            updatedAt: "".to_string(),
            id: "".to_string()
        }, 
        token: "".to_string(), 
        exp: exp
    }
}


pub async fn get_token(email: &str, password: &str) -> Result<Token, reqwest::Error> {
    // info!("{}\n", env::KNIVE_API_URL)
    let client = reqwest::Client::new();
    let url = format!("{}{}", env::KNIVE_API_URL, LOGIN_PATH);
    let mut credentials = HashMap::new();
    
    credentials.insert("email", email);
    credentials.insert("password", password);

    let result =  client.post(url).json(&credentials).send().await?.json().await;

    return result;
}
