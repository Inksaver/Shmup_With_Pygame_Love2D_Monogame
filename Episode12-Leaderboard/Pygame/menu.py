'''
Static class to display menu at start of game, or after lives lost
x key switches to debug mode
l key goes directly to leaderboard during development cycle
'''
import pygame
import shared

def update(keystate:object, key_down:object, player) -> None:
	''' check for user pressing Enter to start '''
	if key_down == pygame.K_RETURN:						# Enter -> start game
		shared.gamestate = shared.gamestates['play']
		player.reset(3)
		shared.score = 0
	if key_down == pygame.K_b:							# b -> toggle background music
		shared.play_music = not shared.play_music
		if shared.audio_present:
			if shared.play_music:
				pygame.mixer.music.play(loops = -1)	
			else:
				pygame.mixer.music.stop()
	if key_down == pygame.K_x:							# x -> toggle debug mode
		shared.debug = not shared.debug
		
	if key_down == pygame.K_l and shared.debug:			# l and debug -> to leaderboard during development
		shared.gamestate = shared.gamestates['leaderboard']
		
def draw(game_title:str) -> None:
	''' show start menu screen '''
	shared.draw_text(shared.screen, game_title, 64, shared.WIDTH * 0.5, shared.HEIGHT * 0.25, 'centre', shared.YELLOW)
	shared.draw_text(shared.screen, "Arrow keys move, space to fire", 22, shared.WIDTH * 0.5, shared.HEIGHT * 0.5)
	shared.draw_text(shared.screen, "Press Enter to begin", 30, shared.WIDTH * 0.5, shared.HEIGHT * 0.65)
	shared.draw_text(shared.screen, "Escape to quit at any time", 18, shared.WIDTH * 0.5, shared.HEIGHT * 0.85)
	shared.draw_text(shared.screen, "Press B to toggle background music", 16, shared.WIDTH * 0.5, shared.HEIGHT * 0.9, 'centre', shared.GREEN)