import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule, FormControl } from '@angular/forms';
import {MovieService} from '../services/movie.service';
import { Movie } from '../models/movie';
import { Observable } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  query = new FormControl('');

  constructor(private router: Router) { }

  ngOnInit() {
  }
  search() {
        this.router.navigate(['/search'], { queryParams: { q: this.query.value } 
      });
    
  }

}
