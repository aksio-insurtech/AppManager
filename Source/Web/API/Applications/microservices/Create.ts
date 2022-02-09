/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/microservices');

export class Create extends Command {
    readonly route: string = '/api/applications/{{applicationId}}/microservices';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    applicationId!: string;
    microserviceId!: string;
    name!: string;
}
