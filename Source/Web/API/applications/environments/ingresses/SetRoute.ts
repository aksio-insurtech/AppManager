/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{environmentId}/ingresses/{ingressId}/route');

export interface ISetRoute {
    applicationId?: string;
}

export class SetRouteValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
    };
}

export class SetRoute extends Command<ISetRoute> implements ISetRoute {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{environmentId}/ingresses/{ingressId}/route';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new SetRouteValidator();

    private _applicationId!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }

    static use(initialValues?: ISetRoute): [SetRoute, SetCommandValues<ISetRoute>] {
        return useCommand<SetRoute, ISetRoute>(SetRoute, initialValues);
    }
}
