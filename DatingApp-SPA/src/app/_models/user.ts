import { Photo } from './photo';

export interface User {
    id: number;
    age: number;
    username: string;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    city: string;
    country: string;
    photoUrl: string;
    interest?: string;
    lookingFor?: string;
    introduction?: string;
    photos?: Photo[];
}
