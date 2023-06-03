import * as prod from './settings.json'
import * as dev from './settings.development.json'

export const config = process.env.NODE_ENV == 'production' ? prod : dev