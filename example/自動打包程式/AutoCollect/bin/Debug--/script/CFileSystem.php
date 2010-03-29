<?php
	class CFileSystem{
		/**
		 * Delete a file, or a folder and its contents
		 *
		 * @author      Aidan Lister <aidan@php.net>
		 * @version     1.0.2
		 * @param       string   $dirname    Directory to delete
		 * @return      bool     Returns TRUE on success, FALSE on failure
		 */
		function delFile($dirname)
		{
			// Sanity check
			if (!file_exists($dirname)) {
				return false;
			}
		 
			// Simple delete for a file
			if (is_file($dirname)) {
				return unlink($dirname);
			}
		 
			// Loop through the folder
			$dir = dir($dirname);
			while (false !== $entry = $dir->read()) {
				// Skip pointers
				if ($entry == '.' || $entry == '..') {
					continue;
				}
		 
				// Recurse
				$this->delFile("$dirname/$entry");
			}
		 
			// Clean up
			$dir->close();
			return rmdir($dirname);
		}

		/**
		 * Move a file, or recursively copy a folder and its contents
		 *
		 * @author      Chui-Wen Chiu
		 * @version     1.0.0
		 * @param       string   $source    Source path
		 * @param       string   $dest      Destination path
		 * @return      bool     Returns TRUE on success, FALSE on failure
		 */
		function moveFile($source, $dest)
		{
				
			// Simple copy for a file
			if (is_file($source)) {				
				return rename($source, $dest);
			}

			// Make destination directory
			if (!is_dir($dest)) {
				mkdir($dest);
			}
		 
			// Loop through the folder
			$dir = dir($source);
			while (false !== $entry = $dir->read()) {
				// Skip pointers
				if ($entry == '.' || $entry == '..') {
					continue;
				}
		 
				// Deep copy directories
				if ($dest !== "$source/$entry") {
					$this->moveFile("$source/$entry", "$dest/$entry");
				}
			}
		 
			// Clean up
			$dir->close();
			return true;
		}

		/**
		 * Copy a file, or recursively copy a folder and its contents
		 *
		 * @author      Aidan Lister <aidan@php.net>
		 * @version     1.0.1
		 * @param       string   $source    Source path
		 * @param       string   $dest      Destination path
		 * @return      bool     Returns TRUE on success, FALSE on failure
		 */
		function copyFile($source, $dest)
		{
				
			// Simple copy for a file
			if (is_file($source)) {
				echo "is-file";
				return copy($source, $dest);
			}

			// Make destination directory
			if (!is_dir($dest)) {
				mkdir($dest);
			}
		 
			// Loop through the folder
			$dir = dir($source);
			while (false !== $entry = $dir->read()) {
				// Skip pointers
				if ($entry == '.' || $entry == '..') {
					continue;
				}
		 
				// Deep copy directories
				if ($dest !== "$source/$entry") {
					$this->copyFile("$source/$entry", "$dest/$entry");
				}
			}
		 
			// Clean up
			$dir->close();
			return true;
		}
			
		/**
		 * Create a directory structure recursively
		 *
		 * @author      Aidan Lister <aidan@php.net>
		 * @version     1.0.0
		 * @param       string   $pathname    The directory structure to create
		 * @return      bool     Returns TRUE on success, FALSE on failure
		 */
		 
		function createFolder($pathname, $mode = null)
		{
			// Check if directory already exists
			if (is_dir($pathname) || empty($pathname)) {
				return true;
			}
		 
			// Ensure a file does not already exist with the same name
			if (is_file($pathname)) {
				trigger_error('mkdir() File exists', E_USER_WARNING);
				return false;
			}
		 
			// Crawl up the directory tree
			$next_pathname = substr($pathname, 0, strrpos($pathname, DIRECTORY_SEPARATOR));
			if ($this->createFolder($next_pathname, $mode)) {
				if (!file_exists($pathname)) {
					return mkdir($pathname, $mode);
				}
			}
		 
			return false;
		}

	}
?>