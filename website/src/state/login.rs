use crate::api::{self, login::*};
use serde::{Serialize, Deserialize};
use chrono::prelude::*;
use dioxus_logger::tracing::{info};

#[derive(Clone, Debug, PartialEq, Serialize, Deserialize)]
pub struct LoginState {
    pub token: Token
}

impl LoginState {
    pub fn new() -> LoginState {
        return LoginState {
            token: newToken()
        };
    }

    pub fn from_token(token: api::login::Token) -> LoginState {
        return LoginState {
            token: token
        };
    }

    pub fn is_valid(&self) -> bool {
        let start = Utc::now().timestamp();
        let token_time = self.token.exp;

        return token_time > start;
    }

    pub fn should_refresh(&self) -> bool {
        let start = Utc::now().timestamp();
        let token_time = self.token.exp;

        return token_time < start - 300;
    }
}