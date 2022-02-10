/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/microservices/{{microserviceId}}/deployables');

export class Create extends Command {
    readonly route: string = '/api/applications/{applicationId}/microservices/{{microserviceId}}/deployables';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;

    get requestArguments(): string[] {
        return [
            'microserviceId',
        ];
    }

    microserviceId!: string;
    deployableId!: string;
    name!: string;
}
