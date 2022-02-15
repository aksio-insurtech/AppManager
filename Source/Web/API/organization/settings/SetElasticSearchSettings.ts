/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/organization/settings/elasticsearch');

export class SetElasticSearchSettings extends Command {
    readonly route: string = '/api/organization/settings/elasticsearch';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;

    get requestArguments(): string[] {
        return [
        ];
    }

    url!: string;
    apiKey!: string;
}
