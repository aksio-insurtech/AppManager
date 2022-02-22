/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image');

export class SetImage extends Command {
    readonly route: string = '/api/applications/{applicationId}/microservices/{{microserviceId}}/deployables/{{deployableId}}/image';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;

    get requestArguments(): string[] {
        return [
            'microserviceId',
            'deployableId',
        ];
    }

    microserviceId!: string;
    deployableId!: string;
    deployableImageName!: string;
}
