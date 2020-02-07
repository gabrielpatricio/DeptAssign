import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { MovieService } from '../services/movie.service';
import { Movie } from '../models/movie';
import { NgbModal} from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  popularMovies$: Observable<Movie[]>;

  constructor(private movieService: MovieService, private modalService: NgbModal) {
  }

  ngOnInit() {
    this.loadPopularMovies();
  }

  /**
   * Method uses MovieService to call API and get popular movies
   * **/
  loadPopularMovies() {
    this.popularMovies$ = this.movieService.getPopularMovies();
    
  }
  open(content) {
    this.modalService.open(content);
  }

}
