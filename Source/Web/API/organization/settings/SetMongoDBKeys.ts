/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings/mongodb');

export class SetMongoDBKeys extends Command {
    readonly route: string = '/api/organization/settings/mongodb';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;

    get requestArguments(): string[] {
        return [
        ];
    }

    publicKey!: string;
    privateKey!: string;
}
