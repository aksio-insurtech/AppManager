/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environmentId}/ingresses/{ingressId}/{{microserviceId}}');

export interface IMakeFrontend {
    applicationId?: string;
    microserviceId?: string;
}

export class MakeFrontendValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        microserviceId: new Validator(),
    };
}

export class MakeFrontend extends Command<IMakeFrontend> implements IMakeFrontend {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environmentId}/ingresses/{ingressId}/{{microserviceId}}';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new MakeFrontendValidator();

    private _applicationId!: string;
    private _microserviceId!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'microserviceId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'microserviceId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }

    static use(initialValues?: IMakeFrontend): [MakeFrontend, SetCommandValues<IMakeFrontend>] {
        return useCommand<MakeFrontend, IMakeFrontend>(MakeFrontend, initialValues);
    }
}
