/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{applicationId}/environments/{environment}/microservices/{{microserviceId}}/remove');

export interface IRemove {
    microserviceId?: string;
}

export class RemoveValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        microserviceId: new Validator(),
    };
}

export class Remove extends Command<IRemove> implements IRemove {
    readonly route: string = '/api/applications/{applicationId}/environments/{environment}/microservices/{{microserviceId}}/remove';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new RemoveValidator();

    private _microserviceId!: string;

    get requestArguments(): string[] {
        return [
            'microserviceId',
        ];
    }

    get properties(): string[] {
        return [
            'microserviceId',
        ];
    }

    get microserviceId(): string {
        return this._microserviceId;
    }

    set microserviceId(value: string) {
        this._microserviceId = value;
        this.propertyChanged('microserviceId');
    }

    static use(initialValues?: IRemove): [Remove, SetCommandValues<IRemove>] {
        return useCommand<Remove, IRemove>(Remove, initialValues);
    }
}
